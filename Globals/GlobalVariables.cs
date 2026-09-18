using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceDemo.Globals
{
    public enum MoneyType
    {
        Dollars,
        Euros,
        Yen
    }

    public enum CheckOutStatus
    {
        InsufficientFunds,
        Succeeded,
        Failed
    }

public static class CurrencyHelper {
    
    private static readonly Dictionary<MoneyType, decimal> RatesFromUsd = new Dictionary<MoneyType, decimal> {
            [MoneyType.Dollars] = 1.00m, [MoneyType.Euros] = 0.92m, [MoneyType.Yen] = 149.50m,
    };

    private static readonly Dictionary<MoneyType, string> Symbols = new Dictionary<MoneyType, string> {
		    [MoneyType.Dollars] = "$", [MoneyType.Euros] = "€", [MoneyType.Yen] = "¥",
    };

   

    public static string GetConversion(decimal usdAmount, MoneyType moneyType) {
        
        
        decimal converted =Math.Round(usdAmount * RatesFromUsd[moneyType], 2);
        
        string symbol = Symbols[moneyType];
        
        return moneyType == MoneyType.Yen
                ? $"{symbol}{converted:N0}"
                : $"{symbol}{converted:N2}";
        
        
    }
    
    
}


}
