﻿using WyCash.Domain;

namespace WyCash.Apllication.Model
{
    public class FinancialTitleModel
    {
        public FinancialTitleModel(FinancialTitle financialTitle)
        {
            Name = financialTitle.Name;
            Quantity = financialTitle.Quantity;
            Valuation = financialTitle.Valuation;
            Currency = financialTitle.Money;
            TotalValue = financialTitle.TotalValue;
            TotalValueInDollar = financialTitle.TotalValueInDollar;
        }

        public string Name { get; }
        public int Quantity { get; }
        public decimal Valuation { get; }
        public Money Currency { get; }
        public decimal TotalValue { get; }
        public decimal TotalValueInDollar { get; }
    }
}
