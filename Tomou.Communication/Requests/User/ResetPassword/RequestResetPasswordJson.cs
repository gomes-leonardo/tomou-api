﻿namespace Tomou.Communication.Requests.User.ForgotPassword;
public class RequestResetPasswordJson
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
