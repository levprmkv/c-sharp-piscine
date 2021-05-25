using System;

namespace Program 
{
    class Program
    {
        static double  Annuity_payment(double sum, double rate, int term)
        {
            double annuity_payment;
            double rate_per_month;
            
            rate_per_month = (rate / 12) / 100;
            annuity_payment = (sum * rate_per_month * Math.Pow((1 + rate_per_month), (double) term)) /
                              (Math.Pow((1 + rate_per_month), (double) term) - 1);
            return (annuity_payment);
        }

        static double Percent(double rate, double reminder, int i)
        {
            double percent;
            percent = reminder * rate * i / 100 / 365;
            return (percent);
        }

        static double DecreaseInAmount(double sum, double rate, double payment, int selectedMonth, int term)
        {
            double sum_of_percents;
            double annuity_payment;
            double reminder;
            double percent;
            int    month;
            DateTime now = DateTime.Now;

            sum_of_percents = 0;
            month = 0;
            reminder = sum;
            annuity_payment = Annuity_payment(sum, rate, term);
            while (month != term)
            {
                percent = Percent(rate, reminder,(int) (now.AddMonths(month + 1) - now.AddMonths(month)).Duration().Days);
                sum_of_percents += percent;
                reminder = reminder - (annuity_payment - percent);
                if (month == selectedMonth - 1)
                {
                    reminder -= payment;
                    annuity_payment = Annuity_payment(reminder, rate, term - selectedMonth);
                }
                if (reminder <= 0)
                    break;
                month++;
            }
            return (sum_of_percents);
        }

        static int CountMonth(double reminder, double rate, double annuity_payment)
        {
            double rate_per_month;
            int month;

            rate_per_month = (rate / 12) / 100;
            month = (int) Math.Log(annuity_payment / (annuity_payment - rate_per_month * reminder), 1 + rate_per_month);
            return (month);
        }

        static double DecreaseInTerm(double sum, double rate, double payment, int selectedMonth, int term)
        {
            DateTime now = DateTime.Now;
            double sum_of_percents;
            double annuity_payment;
            double reminder;
            double percent;
            int month;
            int i;

            i = 0;
            sum_of_percents = 0;
            month = term;
            reminder = sum;
            annuity_payment = Annuity_payment(sum, rate, term);
            while (month != 0)
            {
                percent = Percent(rate, reminder,(int) (now.AddMonths(i + 1) - now.AddMonths(i)).Duration().Days);
                sum_of_percents += percent;
                reminder = reminder - (annuity_payment - percent);
                if (month == term - selectedMonth + 1)
                {
                    reminder -= payment;
                    month = CountMonth(reminder, rate, annuity_payment) + 1;
                }
                if (reminder <= 0)
                    break;
                month--;
                i++;
            }
            return (sum_of_percents);
        }
        static void Output(double decrease_in_amount, double decrease_it_term)
        {
            double difference;
            if (decrease_in_amount > decrease_it_term)
            {
                difference = Math.Round(decrease_in_amount, 2) - Math.Round(decrease_it_term, 2);
                Console.WriteLine("Переплата при уменьшении платежа: {" + Math.Round(decrease_in_amount, 2) + "}р.");
                Console.WriteLine("Переплата при уменьшении срока: {" + Math.Round(decrease_it_term, 2) + "}р.");
                Console.WriteLine("Уменьшение срока выгоднее уменьшения платежа на {" + Math.Round(difference, 2) + "}р.");
            }
            else if (decrease_in_amount < decrease_it_term)
            {
                difference = Math.Round(decrease_it_term, 2) - Math.Round(decrease_in_amount, 2);
                Console.WriteLine("Переплата при уменьшении платежа: {" + Math.Round(decrease_in_amount, 2) + "}р.");
                Console.WriteLine("Переплата при уменьшении срока: {" + Math.Round(decrease_it_term, 2) + "}р.");
                Console.WriteLine("Уменьшение платежа выгоднее уменьшения срока на {" + Math.Round(difference, 2) + "}р.");
            }
            else
            {
                Console.WriteLine("Переплата при уменьшении платежа: {" + Math.Round(decrease_in_amount, 2) + "}р.");
                Console.WriteLine("Переплата при уменьшении срока: {" + Math.Round(decrease_it_term, 2) + "}р.");
            }
        }
        
        static void Main(string[] args)
        {
            double sum;
            double rate;
            double payment;
            double decrease_in_amount;
            double decrease_in_term;
            int    term;
            int    selectedMonth; 
            if (args.Length != 5) 
            { 
                Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
                return;
            }
            Double.TryParse(args[0], out sum);
            Double.TryParse(args[1], out rate);
            Double.TryParse(args[4], out payment);
            Int32.TryParse(args[2], out term);
            Int32.TryParse(args[3], out selectedMonth);
            if (sum <= 0 || rate <= 0 || payment < 0 || term <= 0 || selectedMonth <= 0 || payment >= sum || term < selectedMonth)
            { 
                Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
                return;
            }
            decrease_in_amount = DecreaseInAmount(sum, rate, payment, selectedMonth, term);
            decrease_in_term = DecreaseInTerm(sum, rate, payment, selectedMonth, term);
            Output(decrease_in_amount, decrease_in_term);
        }
    }
}
