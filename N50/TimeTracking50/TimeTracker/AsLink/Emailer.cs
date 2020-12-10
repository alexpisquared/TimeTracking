using AAV.Sys.Ext;
using AAV.Sys.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows;
using TimeTracker.View;

namespace TimeTracker.Common
{
  public static class Emailer
  {
    public static async Task<bool> SmtpSend(string fromEmail, string trgEmail, string msgSubject, string msgBody, string[] attachedFilenames = null, string signatureImage = null)
    {
      try
      {
        using (var mailMessage = new MailMessage(fromEmail, trgEmail, msgSubject, msgBody))
        {
          if (!string.IsNullOrEmpty(signatureImage))
          {
            foreach (var img in signatureImage.Split('|'))
            {
              var contentId = Guid.NewGuid().ToString();
              var html_View = AlternateView.CreateAlternateViewFromString(msgBody.Replace("Logo.png", "cid:" + contentId), null, "text/html"); // (msgBody + "<img src=\"cid:" + contentId + "\" alt=\"MCSD\"><hr />", null, "text/html");
              if (File.Exists(img))
              {
                var imageResource = new LinkedResource(img, new ContentType(MediaTypeNames.Image.Jpeg))
                {
                  ContentId = contentId
                };
                html_View.LinkedResources.Add(imageResource);
              }
              mailMessage.AlternateViews.Add(html_View);
              //var plainView = AlternateView.CreateAlternateViewFromString(msgBody, null, "text/plain");
              //mailMessage.AlternateViews.Add(plainView);
            }
          }

          mailMessage.Subject = msgSubject;
          mailMessage.Body = msgBody;
          mailMessage.IsBodyHtml = msgBody.Substring(0, 1) == "<";

          if (attachedFilenames != null)
          {
            foreach (var fnm in attachedFilenames.Where(r => !string.IsNullOrEmpty(r)))
            {
              if (!File.Exists(fnm)) throw new Exception("Oopsy... Attachment does not exist at: \r\n" + fnm);
              mailMessage.Attachments.Add(new Attachment(fnm));
            }
          }

#if DEBUG_
                    var mailClient = new SmtpClient
                    {
                        UseDefaultCredentials = true,
                        Host = DevOp.IsMyHomePC ? "mail.aei.ca"
                        : "namailbox.danahermail.com", //"namailbox.danahermail.com", // "namail.danahermail.com", //"namailbox.danahermail.com"; // "192.168.116.48";

                        EnableSsl = true,
                        Port = 443
                    };

                    //mailClient.Credentials = //new NetworkCredential("alex.pigida@sciex.com", "Use the other one from ...kj`123");
                    //    new NetworkCredential().GetCredential(mailClient.Host, 25, "Basic");
#else
          //tu: add to App.cfg: <system.net><mailSettings><smtp deliveryMethod="Network" from="test@foo.com"><!--userName="pigida@aei.ca" password="oldSimple"--><!--port="25"--><network host="mail.aei.ca" defaultCredentials="true"/></smtp></mailSettings></system.net>
          var mailClient = new SmtpClient
          {
            UseDefaultCredentials = true,
            Host = VerHelper.IsMyHomePC ? "mail.aei.ca" : "192.168.116.48"
          };
#endif

          await mailClient.SendMailAsync(mailMessage);

        } // using

        return true;
      }
      catch (FormatException ex) { MessageBox.Show(ex.ToString(), "FormatException"); }
      catch (SmtpException ex) { MessageBox.Show(ex.ToString(), "SmtpException"); }
      catch (Exception ex) { MessageBox.Show(ex.ToString(), "Exception"); }

      return false;
    }

    public static (int exitCode, string errMsg) PerpAndShow(string trgEmail, string subj, string body, string hardcopy)
    {
      var exitCode = 0;
      var report = "";
      try
      {
        var psi = new ProcessStartInfo("OUTLOOK.EXE", $"/c ipm.note /m \"{trgEmail}?v=1&subject={subj}&body={body}\" /a \"{hardcopy}\"") //Feb 2020: '?v=1' from https://answers.microsoft.com/en-us/msoffice/forum/all/outlook-command-line-parameters-stopped-working/7abe60b2-be29-4426-bf17-f23e1de9f04b
        {
          UseShellExecute = true, // to pick up OUT LOOK.exe from the path!!!
          WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"0\Ltd\Invoicing")
        };

        using (var process = new Process { StartInfo = psi })
        {
          process.Start();
          do { if (!process.HasExited) process.Refresh(); } while (!process.WaitForExit(3333));
          exitCode = process.ExitCode;
          process.Close();
        }

        return (exitCode, report);
      }
      catch (Win32Exception ex)
      {
        new FallbackEditor($"{trgEmail}\n{subj}\n{hardcopy}\n{body}").Show();
        ex.Log();
    
        //fallbackAction(trgEmail, subj, body, hardcopy);

        return (-1, ex.Message);
      }
      catch (Exception ex) { ex.Log(); throw; }
    }

    static void fallbackAction(string trgEmail, string subj, string body, string hardcopy)
    {
      try
      {
        var workingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"0\Ltd\Invoicing");
        var tempFile = Path.Combine(workingDirectory, "EmailNotSent.txt");
        File.WriteAllText(tempFile, $"{trgEmail}\n{subj}\n{hardcopy}\n{body}");
        var psi = new ProcessStartInfo("NOTEPAD.EXE", tempFile)
        {
          UseShellExecute = true, // to pick up OUT LOOK.exe from the path!!!
          WorkingDirectory = workingDirectory
        };

        using (var process = new Process { StartInfo = psi })
        {
          process.Start();
          do { if (!process.HasExited) process.Refresh(); } while (!process.WaitForExit(3333));
          process.Close();
        }
      }
      catch (Exception ex) { ex.Log(); throw; }
    }
  }
}
//todo: move from app.config to db.config: <network host="192.168.116.48" defaultCredentials="true"/>

