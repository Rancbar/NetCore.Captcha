# NetCore.Captcha
A multi-targeted, cross platform and easy to use Captcha project for using in both of DotNetCore and NetFramework


There is how to use in View:

    @using NetCore.Captcha
    ...
    @Html.MathCaptcha("Refresh", "", "Enter the captcha result here")

And, how to validate in controller's action:

    [NetCore.Captcha.Validation.CaptchaActionValidator("The entered captcha is not valid")]
