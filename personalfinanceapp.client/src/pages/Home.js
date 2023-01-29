import React, { useState } from 'react'
import { Card } from 'primereact/card';
import IncomeChartPie from '../components/IncomeChartPie';
import ExpenseChartPie from '../components/ExpenseChartPie';
import { Calendar } from 'primereact/calendar';
import { Button } from 'primereact/button';
import MonthChartBar from '../components/MonthChartBar';

export default function Home() {
  const firstDayOfMonth = date => new Date(date.getFullYear(), date.getMonth());
  const lastDayOfMonth = date => new Date(date.getFullYear(), date.getMonth() + 1, 0);
  
  const [dateFrom, setDateFrom] = useState(firstDayOfMonth(new Date()));
  const [dateTo, setDateTo] = useState(lastDayOfMonth(new Date()));

  const setCurrentDate = () => {
    setDateFrom(firstDayOfMonth(new Date()));
    setDateTo(lastDayOfMonth(new Date()));
  }
  const nextDate = () => {
    setDateFrom(new Date(dateFrom.getFullYear(), dateFrom.getMonth() + 1));
    setDateTo(new Date(dateTo.getFullYear(), dateTo.getMonth() + 2, 0));
  }
  const prevDate = () => {
    setDateFrom(new Date(dateFrom.getFullYear(), dateFrom.getMonth() - 1));
    setDateTo(new Date(dateTo.getFullYear(), dateTo.getMonth(), 0));
  }

  return (
    <div>
      <Card>
        <div className="flex flex-row justify-content-start align-items-center">
          <div className="p-inputgroup">
            <span className="p-inputgroup-addon">From</span>
            <Calendar value={dateFrom} onChange={e => setDateFrom(e.value)} showIcon dateFormat="dd MM yy"/>
            <span className="p-inputgroup-addon">to</span>
            <Calendar value={dateTo} onChange={e => setDateTo(e.value)} showIcon dateFormat="dd MM yy"/>
            <Button onClick={prevDate} icon="pi pi-arrow-left"/>
            <Button onClick={setCurrentDate} icon="pi pi-fw pi-calendar" label="Current"/>
            <Button onClick={nextDate} icon="pi pi-arrow-right"/>
          </div>
        </div>
      </Card>
      <div className="grid gap-2 pl-2 pr-2 mt-2">
        <Card className='col' title="Incomes">
          <IncomeChartPie from={dateFrom} to={dateTo}/>
        </Card>
        <Card className='col' title="Expenses">
          <ExpenseChartPie from={dateFrom} to={dateTo}/>
        </Card>
      </div>
      <Card className='mt-2'>
        <MonthChartBar from={dateFrom} to={dateTo} />
      </Card>
    </div>
  )
}