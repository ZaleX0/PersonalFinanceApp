import { authHeader } from "../utils/authHeader";

export default class IncomesService {

  async getIncomes() {
    return await fetch("/api/incomes", {
      method: "GET",
      headers: {
        'Authorization': authHeader(),
        'Content-Type': 'application/json'
      }
    });
  }
}