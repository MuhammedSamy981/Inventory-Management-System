import { useEffect, useState } from "react";
import PageHeader, { randomId, formatDate } from "../components/ui/PageHeader";
import Button from "../components/ui/Button";
import DataTable from "../components/ui/DataTable";
import Modal from "../components/ui/Modal";
import ConfirmDialog from "../components/ui/ConfirmDialog";
import { Field, Input, Select } from "../components/ui/Field";
import { inventoriesApi } from "../api/inventoriesApi";
import { peopleApi } from "../api/peopleApi";
import { extractErrorMessage } from "../api/client";
import { useToast } from "../context/ToastContext";

const emptyForm = { id: "", name: "", address: "", managerId: "" };

export default function Inventories() {
  const toast = useToast();
  const [inventories, setInventories] = useState([]);
  const [managers, setManagers] = useState([]);
  const [loading, setLoading] = useState(true);

  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState("");

  const [deleteTarget, setDeleteTarget] = useState(null);
  const [deleting, setDeleting] = useState(false);

  // Report
  const [reportInventoryId, setReportInventoryId] = useState("");
  const [useReportPeriod, setUseReportPeriod] = useState(false);
  const [reportFrom, setReportFrom] = useState("");
  const [reportTo, setReportTo] = useState("");
  const [reportLoading, setReportLoading] = useState(false);
  const [report, setReport] = useState(null);
  const [reportError, setReportError] = useState("");

  const load = async () => {
    setLoading(true);
    try {
      const [invRes, mgrRes] = await Promise.all([
        inventoriesApi.getAll(1, 200),
        peopleApi.getByType("Inventory Manager"),
      ]);
      setInventories(invRes.data || []);
      setManagers(mgrRes.data || []);
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

  const managerName = (id) => managers.find((m) => m.id === id)?.name || `#${id}`;

  const openCreate = () => {
    setEditing(null);
    setForm({ ...emptyForm, id: randomId() });
    setFormError("");
    setModalOpen(true);
  };

  const openEdit = (inv) => {
    setEditing(inv);
    setForm({
      id: inv.id,
      name: inv.name || "",
      address: inv.address || "",
      managerId: inv.managerId || "",
    });
    setFormError("");
    setModalOpen(true);
  };

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormError("");
    setSaving(true);
    const payload = {
      Id: Number(form.id),
      Name: form.name,
      Address: form.address,
      ManagerId: Number(form.managerId),
    };
    try {
      if (editing) {
        await inventoriesApi.update(payload);
        toast.success("Inventory updated.");
      } else {
        await inventoriesApi.create(payload);
        toast.success("Inventory added.");
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
      await inventoriesApi.remove(deleteTarget.id);
      toast.success("Inventory deleted.");
      setDeleteTarget(null);
      load();
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  };

  const runReport = async (e) => {
    e.preventDefault();
    if (!reportInventoryId) return;
    setReportError("");
    setReportLoading(true);
    setReport(null);
    try {
      const { data } = await inventoriesApi.getReport(
        reportInventoryId,
        useReportPeriod ? reportFrom : null,
        useReportPeriod ? reportTo : null
      );
      setReport(data);
    } catch (err) {
      setReportError(extractErrorMessage(err));
    } finally {
      setReportLoading(false);
    }
  };

  const columns = [
    { key: "id", header: "ID", render: (r) => <span className="mono">{r.id}</span> },
    { key: "name", header: "Name" },
    { key: "address", header: "Address" },
    { key: "managerId", header: "Manager", render: (r) => managerName(r.managerId) },
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
        eyebrow="Locations · A-02"
        title="Inventories"
        subtitle="Warehouses and storage locations under your account."
        action={
          <Button variant="accent" onClick={openCreate}>
            + Add inventory
          </Button>
        }
      />

      <div className="panel" style={{ marginBottom: 24 }}>
        <div className="panel-header">
          <div className="panel-title">All inventories</div>
        </div>
        <DataTable columns={columns} rows={inventories} rowKey="id" loading={loading} emptyMessage="No inventories yet — add your first location." />
      </div>

      <div className="panel">
        <div className="panel-header">
          <div className="panel-title">
            <span className="section-label">Report</span> Inventory report
          </div>
        </div>
        <div className="panel-body">
          <form onSubmit={runReport}>
            <div className="report-toolbar">
              <Field label="Inventory" required>
                <Select value={reportInventoryId} onChange={(e) => setReportInventoryId(e.target.value)} required>
                  <option value="">Select an inventory…</option>
                  {inventories.map((inv) => (
                    <option key={inv.id} value={inv.id}>
                      {inv.name} (#{inv.id})
                    </option>
                  ))}
                </Select>
              </Field>

              <div className="field" style={{ minWidth: "auto" }}>
                <label className="checkbox-row" style={{ marginTop: 26 }}>
                  <input
                    type="checkbox"
                    checked={useReportPeriod}
                    onChange={(e) => setUseReportPeriod(e.target.checked)}
                  />
                  Limit to a specific period
                </label>
              </div>

              {useReportPeriod && (
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

          {report && (
            <div>
              <div className="divider" />
              <div className="stat-row">
                <div className="stat-box">
                  <div className="label">Inventory</div>
                  <div className="value" style={{ fontSize: 16 }}>
                    {report.name}
                  </div>
                </div>
                <div className="stat-box">
                  <div className="label">Address</div>
                  <div className="value" style={{ fontSize: 16 }}>
                    {report.address || "—"}
                  </div>
                </div>
                <div className="stat-box">
                  <div className="label">Item lines</div>
                  <div className="value">{report.items?.length || 0}</div>
                </div>
              </div>

              <DataTable
                rowKey="id"
                rows={report.items || []}
                columns={[
                  { key: "id", header: "Item ID", render: (r) => <span className="mono">{r.id}</span> },
                  { key: "name", header: "Name" },
                  { key: "size", header: "Size" },
                  { key: "code", header: "Code" },
                  { key: "quantity", header: "Quantity", render: (r) => r.quantity ?? "—" },
                ]}
                emptyMessage="No items found for this inventory in the selected period."
              />
            </div>
          )}
        </div>
      </div>

      {modalOpen && (
        <Modal
          title={editing ? "Edit inventory" : "Add new inventory"}
          onClose={() => setModalOpen(false)}
          footer={
            <>
              <Button variant="outline" onClick={() => setModalOpen(false)} disabled={saving}>
                Cancel
              </Button>
              <Button variant="accent" onClick={handleSubmit} loading={saving}>
                {editing ? "Save changes" : "Add inventory"}
              </Button>
            </>
          }
        >
          {formError && <div className="alert alert-danger">{formError}</div>}
          <form onSubmit={handleSubmit}>
            <Field label="Inventory ID" required hint={editing ? "" : "Must be unique."}>
              <Input type="number" name="id" value={form.id} onChange={handleChange} required disabled={!!editing} />
            </Field>
            <Field label="Name" required>
              <Input name="name" value={form.name} onChange={handleChange} required />
            </Field>
            <Field label="Address" required>
              <Input name="address" value={form.address} onChange={handleChange} required />
            </Field>
            <Field label="Manager" required hint="Only people of type 'Inventory Manager' are listed.">
              <Select name="managerId" value={form.managerId} onChange={handleChange} required>
                <option value="">Select a manager…</option>
                {managers.map((m) => (
                  <option key={m.id} value={m.id}>
                    {m.name} (#{m.id})
                  </option>
                ))}
              </Select>
            </Field>
          </form>
        </Modal>
      )}

      {deleteTarget && (
        <ConfirmDialog
          title="Delete inventory"
          message={`Are you sure you want to delete "${deleteTarget.name}"? This cannot be undone.`}
          loading={deleting}
          onConfirm={handleDelete}
          onClose={() => setDeleteTarget(null)}
        />
      )}
    </div>
  );
}
