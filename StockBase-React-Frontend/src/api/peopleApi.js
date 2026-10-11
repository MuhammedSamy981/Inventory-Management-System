import client from "./client";

export const peopleApi = {
  getAll: (pageNumber = 1, pageSize = 100) =>
    client.get("/People", { params: { pageNumber, pageSize } }),

  getById: (id) => client.get(`/People/${id}`),

  getByType: (type) => client.get(`/People/type/${encodeURIComponent(type)}`),

  create: (person) => client.post("/People", person),

  update: (person) => client.put("/People", person),

  remove: (id) => client.delete(`/People/${id}`),
};
