import { Button } from 'primereact/button';
import { Chart } from 'primereact/chart';
import { useEffect, useState } from "react";
import IncomeExpenseService from "../services/IncomeExpenseService";
import formatDate from "../utils/formatDate";

export default function MonthChartBar({from, to}) {
  const incomeExpenseService = new IncomeExpenseService();
  const [chartData, setChartData] = useState({});

  const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];

  const getMonthDates = () => {
    

    const fromYear = from.getFullYear();
    const fromMonth = from.getMonth();
    const toYear = to.getFullYear();
    const toMonth = to.getMonth();
    const months = [];

    for(let year = fromYear; year <= toYear; year++) {
        let month = year === fromYear ? fromMonth : 0;
        const monthLimit = year === toYear ? toMonth : 11;

        for(; month <= monthLimit; month++) {
            let date = new Date(year, month);
            months.push(date);
        }
    }
    return months;
  }
  const getIncomeSum = (incomes) => {
    let result = [];
    incomes.reduce((res, value) => {
      const d = new Date(value.date);
      const date = new Date(d.getFullYear(), d.getMonth());
      if (!res[date]) {
        res[date] = { date: date, totalPrice: 0 };
        result.push(res[date]);
      }
      res[date].totalPrice += value.price;
      return res;
    }, {});
    return result.reverse();
  }
  const getExpenseSum = (expenses) => {
    let result = [];
    expenses.reduce((res, value) => {
      const d = new Date(value.date);
      const date = new Date(d.getFullYear(), d.getMonth());
      if (!res[date]) {
        res[date] = { date: date, totalPrice: 0 };
        result.push(res[date]);
      }
      res[date].totalPrice += value.price;
      return res;
    }, {});
    return result.reverse();
  }

  const getChartData = (incomes, expenses) => {
    const monthDates = getMonthDates();
    const incomeSum = getIncomeSum(incomes);
    const expenseSum = getExpenseSum(expenses);
    let result = [];

    monthDates.forEach(monthDate => {
      let income = incomeSum.find(v => formatDate(v.date) === formatDate(monthDate));
      let expense = expenseSum.find(v => formatDate(v.date) === formatDate(monthDate));
      result.push({
        label: `${monthNames[monthDate.getMonth()]} ${monthDate.getFullYear()}`,
        incomeTotal: income ? income.totalPrice : 0,
        expenseTotal: expense ? expense.totalPrice : 0
      })
    });

    setChartData({
      labels: result.map(r=>r.label),
      datasets: [
        {
          label: 'Incomes',
          data: result.map(r=>r.incomeTotal)
        },
        {
          label: 'Expenses',
          data: result.map(r=>r.expenseTotal)
        }
      ]
    });
  }

  const fetchData = async () => {
    let [incomes, expenses] = await Promise.all([
      incomeExpenseService.getIncomes({from: formatDate(from), to: formatDate(to)})
        .then(response => response.json()),
      incomeExpenseService.getExpenses({from: formatDate(from), to: formatDate(to)})
        .then(response => response.json())
    ]);
    getChartData(incomes, expenses);
  }

  useEffect(() => {
    fetchData();
  }, [from, to])
  
  return (
    <Chart type="bar" data={chartData}/>
  )
}