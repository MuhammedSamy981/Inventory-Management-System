import client from "./client";

export const inventoriesApi = {
  getAll: (pageNumber = 1, pageSize = 100) =>
    client.get("/Inventories", { params: { pageNumber, pageSize } }),

  getById: (id) => client.get(`/Inventories/${id}`),

  getAllIds: () => client.get("/Inventories/inventoryIds"),

  create: (inventory) => client.post("/Inventories", inventory),

  update: (inventory) => client.put("/Inventories", inventory),

  remove: (id) => client.delete(`/Inventories/${id}`),

  getReport: (inventoryId, fromDate, toDate) =>
    client.get(`/Inventories/report/${inventoryId}`, {
      params: { fromDate: fromDate || undefined, toDate: toDate || undefined },
    }),
};
