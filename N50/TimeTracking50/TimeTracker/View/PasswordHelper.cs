namespace TimeTracker.View;

public static class PasswordHelper
{
  public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
  public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));
  static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));

  public static void SetAttach(DependencyObject dp, bool value) => dp.SetValue(AttachProperty, value);
  public static bool GetAttach(DependencyObject dp) => (bool)dp.GetValue(AttachProperty);
  public static string GetPassword(DependencyObject dp) => (string)dp.GetValue(PasswordProperty);
  public static void SetPassword(DependencyObject dp, string value) => dp.SetValue(PasswordProperty, value);
  static bool GetIsUpdating(DependencyObject dp) => (bool)dp.GetValue(IsUpdatingProperty);
  static void SetIsUpdating(DependencyObject dp, bool value) => dp.SetValue(IsUpdatingProperty, value);

  static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
  {
    var passwordBox = sender as PasswordBox;
    passwordBox.PasswordChanged -= PasswordChanged;

    if (!GetIsUpdating(passwordBox))
    {
      passwordBox.Password = (string)e.NewValue;
    }

    passwordBox.PasswordChanged += PasswordChanged;
  }
  static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
  {
    if (sender is not PasswordBox passwordBox)
      return;

    if ((bool)e.OldValue)
    {
      passwordBox.PasswordChanged -= PasswordChanged;
    }

    if ((bool)e.NewValue)
    {
      passwordBox.PasswordChanged += PasswordChanged;
    }
  }

  static void PasswordChanged(object sender, RoutedEventArgs e)
  {
    var passwordBox = sender as PasswordBox ?? throw new ArgumentNullException("@@@@@@@@@@@@@@@@");
    SetIsUpdating(passwordBox, true);
    SetPassword(passwordBox, passwordBox.Password);
    SetIsUpdating(passwordBox, false);
  }
}
