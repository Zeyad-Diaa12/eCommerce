using FluentValidation;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Helpers;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> IsValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(
            phoneNumber =>
            {
                try
                {
                    var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                    var number = phoneNumberUtil.Parse(phoneNumber, null);
                    return phoneNumberUtil.IsValidNumber(number);
                }
                catch (NumberParseException)
                {
                    return false;
                }
            }
        ).WithMessage("The phone number is not valid.");
    }
}
