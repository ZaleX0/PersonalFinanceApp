import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { Dropdown } from 'primereact/dropdown';
import { useEffect, useState } from 'react';
import IncomeExpenseService from '../services/IncomeExpenseService';

export default function History() {
  const incomeExpenseService = new IncomeExpenseService();
  const [incomesExpenses, setIncomesExpenses] = useState([]);
  const [showDeleteDialog, setShowDeleteDialog] = useState(false);
  const [query, setQuery] = useState({
    search: "",
    incomeCategoryId: "",
    expenseCategoryId: "",
    dateFrom: "",
    dateTo: ""
  });

  useEffect(() => {
    const fetchIncomes = async () => {
      const response = await incomeExpenseService.getIncomesExpenses("");
      const json = await response.json();
      setIncomesExpenses(json);
    }
    fetchIncomes();
  }, [])
  
  
  const priceBodyTemplate = (rowData) => {
    return rowData.type === 0
      ? <div className="text-green-400">+{rowData.price}</div>
      : <div className="text-red-400">-{rowData.price}</div>
  }

  const actionBodyTemplate = (rowData) => {
    return (
      <div className="flex justify-content-center">
        <Button icon="pi pi-pencil" className="p-button-rounded p-button-info mr-2"/>
        <Button icon="pi pi-trash" className="p-button-rounded p-button-danger"/>
      </div>
    )
  }

  return (
    <Card>
      <DataTable value={incomesExpenses} size="small" paginator rows={10} stripedRows showGridlines>
        <Column field="date" header="Date"/>
        <Column field="price" header="Price" body={priceBodyTemplate}/>
        <Column field="categoryName" header="Category"/>
        <Column field="comment" header="Comment"/>
        <Column body={actionBodyTemplate}/>
      </DataTable>
    </Card>
  )
}