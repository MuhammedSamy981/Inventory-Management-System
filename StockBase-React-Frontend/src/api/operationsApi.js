import client from "./client";

export const operationsApi = {
  getAll: (pageNumber = 1, pageSize = 100) =>
    client.get("/Operations", { params: { pageNumber, pageSize } }),

  getById: (id) => client.get(`/Operations/${id}`),

  getByType: (type,pageNumber = 1, pageSize = 100) => 
    client.get(`/Operations/type/${encodeURIComponent(type)}`, { params: { pageNumber, pageSize } }),

  create: (operation) => client.post("/Operations", operation),

  update: (operation) => client.put("/Operations", operation),

  remove: (id) => client.delete(`/Operations/${id}`),

  createTransfer: (transfer) => client.post("/Operations/transfer", transfer),
};
