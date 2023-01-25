import { Card } from 'primereact/card'
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { ColorPicker } from 'primereact/colorpicker';
import React, { useEffect, useState } from 'react'
import CategoriesService from '../services/CategoriesService'
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';

export default function Categories() {
  const categoriesService = new CategoriesService();

  const [incomeCategories, setIncomeCategories] = useState([]);
  const [expenseCategories, setExpenseCategories] = useState([]);

  useEffect(() => {
    const fetchData  = async () => {
      await categoriesService.getIncomeCategories()
        .then(response => response.json())
        .then(data => setIncomeCategories(data));
      await categoriesService.getExpenseCategories()
        .then(response => response.json())
        .then(data => setExpenseCategories(data));
    }
    fetchData();
  }, [])

  
  const saveBody = (item) => {
    return <div className="flex justify-content-end align-items-center">
      <Button disabled icon="pi pi-save" className="p-button-rounded p-button-info ml-2"/>
    </div>
  }
  const colorFieldBody = (item) => <ColorPicker value={item.color} defaultColor="999999" onChange={e => item.color = e.value}/>
  const textEditor = (options) => <InputText type="text" value={options.value} onChange={(e) => options.editorCallback(e.target.value)}/>
  const onCellEditComplete = (event) => {
    let { rowData, value, newValue, field } = event;
    newValue = newValue.trim();
    if (newValue === null || newValue === '' || value === newValue) {
      return;
    }
    rowData[field] = newValue;

    console.log(rowData);
  }
  return (
    <div className="grid p-2 gap-2">
      <Card header="Income categories" className="col">
      <DataTable value={incomeCategories} editMode="cell">
          <Column field="name" header="Name"/>
          <Column field="color" header="Color"/>
        </DataTable>
      </Card>
      <Card header="Expense categories" className="col">
        <Button onClick={()=>console.log(expenseCategories)}/>
        <DataTable value={expenseCategories}>
          <Column field="color" body={colorFieldBody}/>
          <Column field="name" editor={textEditor} onCellEditComplete={onCellEditComplete}/>
          <Column field="saveButton" body={saveBody}/>
        </DataTable>
      </Card>
    </div>
  )
}