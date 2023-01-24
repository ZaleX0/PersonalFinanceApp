import { authHeader } from "../utils/authHeader";

export default class IncomeExpenseService {

  async getIncomesExpenses(query) {
    let url = "/api/incomeexpense";
    if (query !== undefined) {
      const params = `?search=${query}`;
      url += params;
    }
    return await fetch(url, {
      method: "GET",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }

  async deleteIncomeExpense(incomeExpense) {
    const url = incomeExpense.type === 0
      ? `/api/incomes/${incomeExpense.id}`
      : `/api/expenses/${incomeExpense.id}`;

    return await fetch(url, {
      method: "DELETE",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }

  async updateIncomeExpense(incomeExpense) {
    const url = incomeExpense.type === 0
      ? `/api/incomes/${incomeExpense.id}`
      : `/api/expenses/${incomeExpense.id}`;

    return await fetch(url, {
      method: "PUT",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        categoryId: incomeExpense.categoryId,
        price: incomeExpense.price,
        comment: incomeExpense.comment,
        date: incomeExpense.date
      })
    });
  }
}