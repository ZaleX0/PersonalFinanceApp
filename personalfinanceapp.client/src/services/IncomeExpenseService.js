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
}