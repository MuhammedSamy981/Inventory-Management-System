import client from "./client";

export const itemsApi = {
  getAll: (pageNumber = 1, pageSize = 100) =>
    client.get("/Items", { params: { pageNumber, pageSize } }),

  getById: (id) => client.get(`/Items/${id}`),

  create: (item) => client.post("/Items", item),

  update: (item) => client.put("/Items", item),

  remove: (id) => client.delete(`/Items/${id}`),

  // itemId + optional date range as query, inventoryIds array as body
  getReport: (itemId, inventoryIds, fromDate, toDate) =>
    client.post("/Items/report", inventoryIds, {
      params: {
        itemId,
        fromDate: fromDate || undefined,
        toDate: toDate || undefined,
      },
    }),

  // period expressed as a lookback window (days/months/years back from today)
  getStored: (inventoryId, daysCount, monthsCount, yearsCount) =>
    client.get(`/Items/storage/${inventoryId}/${daysCount}/${monthsCount}/${yearsCount}`),

  getExpired: (inventoryId, daysCount, monthsCount, yearsCount) =>
    client.get(`/Items/expired/${inventoryId}/${daysCount}/${monthsCount}/${yearsCount}`),

  getMovements: (inventoryIds, fromDate, toDate) =>
    client.post("/Items/movements/listInventoryIds", inventoryIds, {
      params: {
        fromDate: fromDate || undefined,
        toDate: toDate || undefined,
      },
    }),
};
