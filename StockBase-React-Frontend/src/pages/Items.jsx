import { useEffect, useState } from "react";
import PageHeader, { randomId } from "../components/ui/PageHeader";
import Button from "../components/ui/Button";
import DataTable from "../components/ui/DataTable";
import Modal from "../components/ui/Modal";
import ConfirmDialog from "../components/ui/ConfirmDialog";
import { Field, Input, Select } from "../components/ui/Field";
import MultiSelect from "../components/ui/MultiSelect";
import { itemsApi } from "../api/itemsApi";
import { inventoriesApi } from "../api/inventoriesApi";
import { extractErrorMessage } from "../api/client";
import { useToast } from "../context/ToastContext";

const emptyForm = { id: "", name: "", code: "", size: "" };

export default function Items() {
  const toast = useToast();
  const [items, setItems] = useState([]);
  const [inventories, setInventories] = useState([]);
  const [loading, setLoading] = useState(true);

  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState("");

  const [deleteTarget, setDeleteTarget] = useState(null);
  const [deleting, setDeleting] = useState(false);

  // --- Item report (one or more inventories, optional date range) ---
  const [reportItemId, setReportItemId] = useState("");
  const [reportInvIds, setReportInvIds] = useState([]);
  const [reportUsePeriod, setReportUsePeriod] = useState(false);
  const [reportFrom, setReportFrom] = useState("");
  const [reportTo, setReportTo] = useState("");
  const [reportLoading, setReportLoading] = useState(false);
  const [reportError, setReportError] = useState("");
  const [reportResult, setReportResult] = useState(null);

  // --- Stored items (single inventory, lookback period) ---
  const [storedInvId, setStoredInvId] = useState("");
  const [storedUsePeriod, setStoredUsePeriod] = useState(false);
  const [storedDays, setStoredDays] = useState(0);
  const [storedMonths, setStoredMonths] = useState(0);
  const [storedYears, setStoredYears] = useState(1);
  const [storedLoading, setStoredLoading] = useState(false);
  const [storedError, setStoredError] = useState("");
  const [storedRows, setStoredRows] = useState(null);

  // --- Expired items (single inventory, lookback period) ---
  const [expiredInvId, setExpiredInvId] = useState("");
  const [expiredUsePeriod, setExpiredUsePeriod] = useState(false);
  const [expiredDays, setExpiredDays] = useState(0);
  const [expiredMonths, setExpiredMonths] = useState(0);
  const [expiredYears, setExpiredYears] = useState(1);
  const [expiredLoading, setExpiredLoading] = useState(false);
  const [expiredError, setExpiredError] = useState("");
  const [expiredRows, setExpiredRows] = useState(null);

  // --- Movements report (one or more inventories, optional date range) ---
  const [moveInvIds, setMoveInvIds] = useState([]);
  const [moveUsePeriod, setMoveUsePeriod] = useState(false);
  const [moveFrom, setMoveFrom] = useState("");
  const [moveTo, setMoveTo] = useState("");
  const [moveLoading, setMoveLoading] = useState(false);
  const [moveError, setMoveError] = useState("");
  const [moveResult, setMoveResult] = useState(null);

  const load = async () => {
    setLoading(true);
    try {
      const [itemsRes, invRes] = await Promise.all([itemsApi.getAll(1, 200), inventoriesApi.getAll(1, 200)]);
      setItems(itemsRes.data || []);
      setInventories(invRes.data || []);
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const openCreate = () => {
    setEditing(null);
    setForm({ ...emptyForm, id: randomId() });
    setFormError("");
    setModalOpen(true);
  };

  const openEdit = (item) => {
    setEditing(item);
    setForm({ id: item.id, name: item.name || "", code: item.code || "", size: item.size || "" });
    setFormError("");
    setModalOpen(true);
  };

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormError("");
    setSaving(true);
    const payload = { Id: Number(form.id), Name: form.name, Code: form.code, Size: form.size };
    try {
      if (editing) {
        await itemsApi.update(payload);
        toast.success("Item updated.");
      } else {
        await itemsApi.create(payload);
        toast.success("Item added.");
      }
      setModalOpen(false);
      load();
    } catch (err) {
      setFormError(extractErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    setDeleting(true);
    try {
      await itemsApi.remove(deleteTarget.id);
      toast.success("Item deleted.");
      setDeleteTarget(null);
      load();
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  };

  const runItemReport = async (e) => {
    e.preventDefault();
    if (!reportItemId || reportInvIds.length === 0) return;
    setReportError("");
    setReportLoading(true);
    setReportResult(null);
    try {
      const { data } = await itemsApi.getReport(
        Number(reportItemId),
        reportInvIds,
        reportUsePeriod ? reportFrom : null,
        reportUsePeriod ? reportTo : null
      );
      setReportResult(data);
    } catch (err) {
      setReportError(extractErrorMessage(err));
    } finally {
      setReportLoading(false);
    }
  };

  const runStoredReport = async (e) => {
    e.preventDefault();
    if (!storedInvId) return;
    setStoredError("");
    setStoredLoading(true);
    setStoredRows(null);
    try {
      const { data } = await itemsApi.getStored(
        Number(storedInvId),
        storedUsePeriod ? Number(storedDays) || 0 : 0,
        storedUsePeriod ? Number(storedMonths) || 0 : 0,
        storedUsePeriod ? Number(storedYears) || 0 : 100
      );
      setStoredRows(data || []);
    } catch (err) {
      setStoredError(extractErrorMessage(err));
    } finally {
      setStoredLoading(false);
    }
  };

  const runExpiredReport = async (e) => {
    e.preventDefault();
    if (!expiredInvId) return;
    setExpiredError("");
    setExpiredLoading(true);
    setExpiredRows(null);
    try {
      const { data } = await itemsApi.getExpired(
        Number(expiredInvId),
        expiredUsePeriod ? Number(expiredDays) || 0 : 0,
        expiredUsePeriod ? Number(expiredMonths) || 0 : 0,
        expiredUsePeriod ? Number(expiredYears) || 0 : 100
      );
      setExpiredRows(data || []);
    } catch (err) {
      setExpiredError(extractErrorMessage(err));
    } finally {
      setExpiredLoading(false);
    }
  };

  const runMoveReport = async (e) => {
    e.preventDefault();
    if (moveInvIds.length === 0) return;
    setMoveError("");
    setMoveLoading(true);
    setMoveResult(null);
    try {
      const { data } = await itemsApi.getMovements(
        moveInvIds,
        moveUsePeriod ? moveFrom : null,
        moveUsePeriod ? moveTo : null
      );
      setMoveResult(Array.isArray(data) ? data : [data].filter(Boolean));
    } catch (err) {
      setMoveError(extractErrorMessage(err));
    } finally {
      setMoveLoading(false);
    }
  };

  const columns = [
    { key: "id", header: "ID", render: (r) => <span className="mono">{r.id}</span> },
    { key: "name", header: "Name" },
    { key: "code", header: "Code", render: (r) => <span className="mono">{r.code}</span> },
    { key: "size", header: "Size" },
    {
      key: "actions",
      header: "",
      align: "right",
      render: (r) => (
        <div className="cell-actions">
          <Button size="sm" variant="outline" onClick={() => openEdit(r)}>
            Edit
          </Button>
          <Button size="sm" variant="danger" onClick={() => setDeleteTarget(r)}>
            Delete
          </Button>
        </div>
      ),
    },
  ];

  return (
    <div>
      <PageHeader
        eyebrow="Catalog · A-03"
        title="Items"
        subtitle="Stock keeping units tracked across your inventories."
        action={
          <Button variant="accent" onClick={openCreate}>
            + Add item
          </Button>
        }
      />

      <div className="panel" style={{ marginBottom: 24 }}>
        <div className="panel-header">
          <div className="panel-title">All items</div>
        </div>
        <DataTable columns={columns} rows={items} rowKey="id" loading={loading} emptyMessage="No items yet — add your first item." />
      </div>

      {/* Item report */}
      <div className="panel" style={{ marginBottom: 24 }}>
        <div className="panel-header">
          <div className="panel-title">
            <span className="section-label">Report</span> Item report across inventories
          </div>
        </div>
        <div className="panel-body">
          <form onSubmit={runItemReport}>
            <div className="field-row">
              <Field label="Item" required>
                <Select value={reportItemId} onChange={(e) => setReportItemId(e.target.value)} required>
                  <option value="">Select an item…</option>
                  {items.map((it) => (
                    <option key={it.id} value={it.id}>
                      {it.name} (#{it.id})
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Inventories" required hint="Select one or more inventories to check.">
                <MultiSelect options={inventories} selected={reportInvIds} onChange={setReportInvIds} placeholder="Add an inventory…" />
              </Field>
            </div>
            <div className="report-toolbar">
              <div className="field" style={{ minWidth: "auto" }}>
                <label className="checkbox-row" style={{ marginTop: 4 }}>
                  <input type="checkbox" checked={reportUsePeriod} onChange={(e) => setReportUsePeriod(e.target.checked)} />
                  Limit to a specific period
                </label>
              </div>
              {reportUsePeriod && (
                <>
                  <Field label="From date">
                    <Input type="date" value={reportFrom} onChange={(e) => setReportFrom(e.target.value)} />
                  </Field>
                  <Field label="To date">
                    <Input type="date" value={reportTo} onChange={(e) => setReportTo(e.target.value)} />
                  </Field>
                </>
              )}
              <Button type="submit" variant="primary" loading={reportLoading}>
                Generate report
              </Button>
            </div>
          </form>

          {reportError && <div className="alert alert-danger">{reportError}</div>}

          {reportResult && (
            <div>
              <div className="divider" />
              <div className="stat-row">
                <div className="stat-box">
                  <div className="label">Item</div>
                  <div className="value" style={{ fontSize: 16 }}>
                    {reportResult.name}
                  </div>
                </div>
                <div className="stat-box">
                  <div className="label">Code</div>
                  <div className="value" style={{ fontSize: 16 }}>
                    {reportResult.code}
                  </div>
                </div>
                <div className="stat-box">
                  <div className="label">Size</div>
                  <div className="value" style={{ fontSize: 16 }}>
                    {reportResult.size}
                  </div>
                </div>
              </div>
              <DataTable
                rowKey="id"
                rows={reportResult.inventories || []}
                columns={[
                  { key: "id", header: "Inventory ID", render: (r) => <span className="mono">{r.id}</span> },
                  { key: "name", header: "Inventory name" },
                  { key: "address", header: "Address" },
                  { key: "quantity", header: "Quantity", render: (r) => r.quantity ?? "—" },
                ]}
                emptyMessage="This item was not found in the selected inventories for this period."
              />
            </div>
          )}
        </div>
      </div>

      <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 24, marginBottom: 24 }}>
        {/* Stored items */}
        <div className="panel">
          <div className="panel-header">
            <div className="panel-title">
              <span className="section-label">Report</span> Items stored
            </div>
          </div>
          <div className="panel-body">
            <form onSubmit={runStoredReport}>
              <Field label="Inventory" required>
                <Select value={storedInvId} onChange={(e) => setStoredInvId(e.target.value)} required>
                  <option value="">Select an inventory…</option>
                  {inventories.map((inv) => (
                    <option key={inv.id} value={inv.id}>
                      {inv.name} (#{inv.id})
                    </option>
                  ))}
                </Select>
              </Field>
              <label className="checkbox-row" style={{ marginBottom: 14 }}>
                <input type="checkbox" checked={storedUsePeriod} onChange={(e) => setStoredUsePeriod(e.target.checked)} />
                Limit to a period (looking back from today)
              </label>
              {storedUsePeriod && (
                <div className="field-row" style={{ gridTemplateColumns: "1fr 1fr 1fr" }}>
                  <Field label="Years">
                    <Input type="number" min={0} value={storedYears} onChange={(e) => setStoredYears(e.target.value)} />
                  </Field>
                  <Field label="Months">
                    <Input type="number" min={0} value={storedMonths} onChange={(e) => setStoredMonths(e.target.value)} />
                  </Field>
                  <Field label="Days">
                    <Input type="number" min={0} value={storedDays} onChange={(e) => setStoredDays(e.target.value)} />
                  </Field>
                </div>
              )}
              <Button type="submit" variant="primary" loading={storedLoading}>
                Run report
              </Button>
            </form>

            {storedError && <div className="alert alert-danger" style={{ marginTop: 16 }}>{storedError}</div>}

            {storedRows && (
              <div style={{ marginTop: 16 }}>
                <DataTable
                  rowKey="id"
                  rows={storedRows}
                  columns={[
                    { key: "name", header: "Item" },
                    { key: "size", header: "Size" },
                    { key: "quantity", header: "Qty", render: (r) => r.quantity ?? "—" },
                    {
                      key: "storageDate",
                      header: "Stored on",
                      render: (r) => (r.storageDate ? new Date(r.storageDate).toLocaleDateString() : "—"),
                    },
                  ]}
                  emptyMessage="No stored items found for this period."
                />
              </div>
            )}
          </div>
        </div>

        {/* Expired items */}
        <div className="panel">
          <div className="panel-header">
            <div className="panel-title">
              <span className="section-label">Report</span> Items expired
            </div>
          </div>
          <div className="panel-body">
            <form onSubmit={runExpiredReport}>
              <Field label="Inventory" required>
                <Select value={expiredInvId} onChange={(e) => setExpiredInvId(e.target.value)} required>
                  <option value="">Select an inventory…</option>
                  {inventories.map((inv) => (
                    <option key={inv.id} value={inv.id}>
                      {inv.name} (#{inv.id})
                    </option>
                  ))}
                </Select>
              </Field>
              <label className="checkbox-row" style={{ marginBottom: 14 }}>
                <input type="checkbox" checked={expiredUsePeriod} onChange={(e) => setExpiredUsePeriod(e.target.checked)} />
                Limit to a period (looking back from today)
              </label>
              {expiredUsePeriod && (
                <div className="field-row" style={{ gridTemplateColumns: "1fr 1fr 1fr" }}>
                  <Field label="Years">
                    <Input type="number" min={0} value={expiredYears} onChange={(e) => setExpiredYears(e.target.value)} />
                  </Field>
                  <Field label="Months">
                    <Input type="number" min={0} value={expiredMonths} onChange={(e) => setExpiredMonths(e.target.value)} />
                  </Field>
                  <Field label="Days">
                    <Input type="number" min={0} value={expiredDays} onChange={(e) => setExpiredDays(e.target.value)} />
                  </Field>
                </div>
              )}
              <Button type="submit" variant="primary" loading={expiredLoading}>
                Run report
              </Button>
            </form>

            {expiredError && <div className="alert alert-danger" style={{ marginTop: 16 }}>{expiredError}</div>}

            {expiredRows && (
              <div style={{ marginTop: 16 }}>
                <DataTable
                  rowKey="id"
                  rows={expiredRows}
                  columns={[
                    { key: "name", header: "Item" },
                    { key: "size", header: "Size" },
                    { key: "quantity", header: "Qty", render: (r) => r.quantity ?? "—" },
                    {
                      key: "expiryDate",
                      header: "Expires on",
                      render: (r) => (r.expiryDate ? new Date(r.expiryDate).toLocaleDateString() : "—"),
                    },
                  ]}
                  emptyMessage="No expired items found for this period."
                />
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Movements report */}
      <div className="panel">
        <div className="panel-header">
          <div className="panel-title">
            <span className="section-label">Report</span> Items movement report
          </div>
        </div>
        <div className="panel-body">
          <form onSubmit={runMoveReport}>
            <Field label="Inventories" required hint="Select one or more inventories to include.">
              <MultiSelect options={inventories} selected={moveInvIds} onChange={setMoveInvIds} placeholder="Add an inventory…" />
            </Field>
            <div className="report-toolbar">
              <div className="field" style={{ minWidth: "auto" }}>
                <label className="checkbox-row" style={{ marginTop: 4 }}>
                  <input type="checkbox" checked={moveUsePeriod} onChange={(e) => setMoveUsePeriod(e.target.checked)} />
                  Limit to a specific period
                </label>
              </div>
              {moveUsePeriod && (
                <>
                  <Field label="From date">
                    <Input type="date" value={moveFrom} onChange={(e) => setMoveFrom(e.target.value)} />
                  </Field>
                  <Field label="To date">
                    <Input type="date" value={moveTo} onChange={(e) => setMoveTo(e.target.value)} />
                  </Field>
                </>
              )}
              <Button type="submit" variant="primary" loading={moveLoading}>
                Generate report
              </Button>
            </div>
          </form>

          {moveError && <div className="alert alert-danger">{moveError}</div>}

          {moveResult && (
            <div>
              <div className="divider" />
              {moveResult.length === 0 && <div className="empty-state">No movements found for this selection.</div>}
              {moveResult.map((inv) => (
                <div key={inv.inventoryId} style={{ marginBottom: 24 }}>
                  <div style={{ display: "flex", alignItems: "center", gap: 10, marginBottom: 10 }}>
                    <span className="badge badge-neutral">{inv.inventoryName}</span>
                    <span style={{ fontSize: 12.5, color: "var(--text-muted)" }}>{inv.inventoryAddress}</span>
                  </div>
                  <DataTable
                    rowKey="id"
                    rows={inv.items || []}
                    columns={[
                      { key: "name", header: "Item" },
                      { key: "moveName", header: "Movement" },
                      { key: "quantity", header: "Qty", render: (r) => r.quantity ?? "—" },
                      {
                        key: "moveDate",
                        header: "Date",
                        render: (r) => (r.moveDate ? new Date(r.moveDate).toLocaleDateString() : "—"),
                      },
                    ]}
                    emptyMessage="No movements in this inventory for the selected period."
                  />
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      {modalOpen && (
        <Modal
          title={editing ? "Edit item" : "Add new item"}
          onClose={() => setModalOpen(false)}
          footer={
            <>
              <Button variant="outline" onClick={() => setModalOpen(false)} disabled={saving}>
                Cancel
              </Button>
              <Button variant="accent" onClick={handleSubmit} loading={saving}>
                {editing ? "Save changes" : "Add item"}
              </Button>
            </>
          }
        >
          {formError && <div className="alert alert-danger">{formError}</div>}
          <form onSubmit={handleSubmit}>
            <Field label="Item ID" required hint={editing ? "" : "Must be unique."}>
              <Input type="number" name="id" value={form.id} onChange={handleChange} required disabled={!!editing} />
            </Field>
            <Field label="Name" required>
              <Input name="name" value={form.name} onChange={handleChange} required />
            </Field>
            <div className="field-row">
              <Field label="Code" required>
                <Input name="code" value={form.code} onChange={handleChange} required />
              </Field>
              <Field label="Size" required>
                <Input name="size" value={form.size} onChange={handleChange} required />
              </Field>
            </div>
          </form>
        </Modal>
      )}

      {deleteTarget && (
        <ConfirmDialog
          title="Delete item"
          message={`Are you sure you want to delete "${deleteTarget.name}"? This cannot be undone.`}
          loading={deleting}
          onConfirm={handleDelete}
          onClose={() => setDeleteTarget(null)}
        />
      )}
    </div>
  );
}
