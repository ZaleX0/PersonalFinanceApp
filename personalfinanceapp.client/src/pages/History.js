import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Column } from 'primereact/column';
import { DataTable } from 'primereact/datatable';
import { InputText } from 'primereact/inputtext';
import { useState } from 'react';
import IncomesService from '../services/IncomesService';

export default function History() {
  const incomesService = new IncomesService();
  const [incomes, setIncomes] = useState({});

  const fetchIncomes = async () => {
    const response = await incomesService.getIncomes();
    const json = await response.json();
    setIncomes(json);
  }



  return (
    <Card>
      <Button label="test" onClick={fetchIncomes}/>
      <DataTable value={incomes} size="small" stripedRows>
        <Column sortable field="date" header="Date"/>
        <Column sortable field="price" header="Price"/>
        <Column sortable field="categoryName" header="Category"/>
        <Column field="comment" header="Comment"/>
        <Column body={<Button />} />
      </DataTable>
    </Card>
  )
}