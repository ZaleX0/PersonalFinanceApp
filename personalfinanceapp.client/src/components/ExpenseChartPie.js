import { Chart } from 'primereact/chart';
import { useEffect, useState } from 'react';
import IncomeExpenseService from '../services/IncomeExpenseService';
import formatDate from '../utils/formatDate';

export default function ExpenseChartPie({ from, to }) {
  const incomeExpenseService = new IncomeExpenseService();
  const [loading, setLoading] = useState(true);
  const [chartData, setChartData] = useState({});
  const [chartOptions, setChartOptions] = useState({});

  const [sum, setSum] = useState(0);

  const getChartData = (expenses) => {
    if (expenses.length === 0) {
      return;
    }
    setSum(expenses.map(value => value.price).reduce((acc, price) => acc + price));
    let result = []
    expenses.reduce((res, value) => {
      if (!res[value.categoryId]) {
        res[value.categoryId] = { id: value.categoryId, name: value.categoryName, totalPrice: 0 };
        result.push(res[value.categoryId]);
      }
      res[value.categoryId].totalPrice += value.price;
      return res;
    }, {});

    return {
      labels: result.map(value => value.name),
      datasets: [{
        data: result.map(value => value.totalPrice),
      }]
    };
  }

  const options = {
    plugins: {
      legend: {
        labels: {
          color: "#ffffff",
          usePointStyle: true
        }
      }
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      setSum('');
      setLoading(true);
      await incomeExpenseService.getExpenses({from: formatDate(from), to: formatDate(to)})
        .then(response => response.json())
        .then(json => {
          const data = getChartData(json);
          setChartData(data);
          setChartOptions(options);
          setLoading(false);
        });
    }
    fetchData();
  }, [from, to])
  
  const plugins = [{
    beforeDraw: function(chart) {
     var width = chart.width,
         height = chart.height,
         ctx = chart.ctx;
         ctx.restore();
         var fontSize = (height / 200).toFixed(2);
         ctx.font = fontSize + "em arial";
         ctx.fillStyle = "#ef4444";
         ctx.textBaseline = "top";
         var text = `${sum}`,
         textX = Math.round((width - ctx.measureText(text).width) / 2),
         textY = height / 2;
         ctx.fillText(text, textX, textY);
         ctx.save();
    } 
  }];

  return (
      <Chart type="doughnut" data={loading?null:chartData} options={chartOptions} plugins={plugins}/>
  )
}