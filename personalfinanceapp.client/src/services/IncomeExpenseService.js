import { authHeader } from "../utils/authHeader";

export default class IncomeExpenseService {

  async getIncomes(query) {
    let url = "/api/incomes";
    if (query.from !== undefined && query.to !== undefined) {
      url += `?dateFrom=${query.from}&dateTo=${query.to}`;
    }
    else if (query.from !== undefined) {
      url += `?dateFrom=${query.from}`;
    }
    else if (query.to !== undefined) {
      url += `?dateTo=${query.to}`;
    }
    return await fetch(url, {
      method: "GET",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
  async getExpenses(query) {
    let url = "/api/expenses";
    if (query.from !== undefined && query.to !== undefined) {
      url += `?dateFrom=${query.from}&dateTo=${query.to}`;
    }
    else if (query.from !== undefined) {
      url += `?dateFrom=${query.from}`;
    }
    else if (query.to !== undefined) {
      url += `?dateTo=${query.to}`;
    }
    return await fetch(url, {
      method: "GET",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
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
  async addIncomeExpense(incomeExpense) {
    const url = incomeExpense.type === 0
      ? `/api/incomes`
      : `/api/expenses`;

    return await fetch(url, {
      method: "POST",
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