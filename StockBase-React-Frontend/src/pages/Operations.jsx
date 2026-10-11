import { useEffect, useState } from "react";
import PageHeader, { randomId, formatDate, toDateInputValue } from "../components/ui/PageHeader";
import Button from "../components/ui/Button";
import DataTable from "../components/ui/DataTable";
import Modal from "../components/ui/Modal";
import ConfirmDialog from "../components/ui/ConfirmDialog";
import { Field, Input, Select } from "../components/ui/Field";
import { operationsApi } from "../api/operationsApi";
import { itemsApi } from "../api/itemsApi";
import { peopleApi } from "../api/peopleApi";
import { inventoriesApi } from "../api/inventoriesApi";
import { extractErrorMessage } from "../api/client";
import { useToast } from "../context/ToastContext";

const OPERATION_TYPES = ["Supply Permit", "Exchange Permit"];

const emptyForm = {
  type: "",
  id: "",
  date: new Date().toISOString().slice(0, 10),
  itemId: "",
  quantity: "",
  proDate: "",
  expiryDate: "",
  personId: "",
  inventoryId: "",
};

function typeBadgeClass(type) {
  return type === "Supply Permit" ? "badge-supply" : "badge-exchange";
}

export default function Operations() {
  const toast = useToast();
  const [operations, setOperations] = useState([]);
  const [items, setItems] = useState([]);
  const [people, setPeople] = useState([]);
  const [inventories, setInventories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [typeFilter, setTypeFilter] = useState("All");

  const [modalOpen, setModalOpen] = useState(false);
  const [peopleByOperationType, setPeopleByOperationType] = useState([]);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState("");

  const [deleteTarget, setDeleteTarget] = useState(null);
  const [deleting, setDeleting] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const [opsRes, itemsRes, peopleRes, invRes] = await Promise.all([
        operationsApi.getAll(1, 200),
        itemsApi.getAll(1, 200),
        peopleApi.getAll(1, 200),
        inventoriesApi.getAll(1, 200),
      ]);
      setOperations(opsRes.data || []);
      setItems(itemsRes.data || []);
      setPeople(peopleRes.data || []);
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

  const itemName = (id) => items.find((i) => i.id === id)?.name || `#${id}`;
  const personName = (id) => people.find((p) => p.id === id)?.name || `#${id}`;
  const inventoryName = (id) => inventories.find((i) => i.id === id)?.name || `#${id}`;

  const openCreate = () => {
    setEditing(null);
    setPeopleByOperationType([]);
    setForm({ ...emptyForm, id: randomId(), date: new Date().toISOString().slice(0, 10) });
    setFormError("");
    setModalOpen(true);
  };

  const openEdit = (op) => {
    setEditing(op);
    handleTypeChange({ target: { name: "type", value: op.type } });
    setForm({
      type: op.type || "Supply Permit",
      id: op.id,
      date: toDateInputValue(op.date) || new Date().toISOString().slice(0, 10),
      itemId: op.itemId ?? "",
      quantity: op.quantity ?? "",
      proDate: toDateInputValue(op.proDate),
      expiryDate: toDateInputValue(op.expiryDate),
      personId: op.personId ?? "",
      inventoryId: op.inventoryId ?? "",
    });
    setFormError("");
    setModalOpen(true);
  };


  const handleTypeChange = async (e) => {

      setForm({ ...form, [e.target.name]: e.target.value });
      setPeopleByOperationType([]);
      const peopleRes = await 
      peopleApi.getByType(e.target.value==="Supply Permit"?"Supplier":"Client");
      setPeopleByOperationType(peopleRes.data || []);

  };

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormError("");
    setSaving(true);
    const payload = {
      Type: form.type,
      Id: Number(form.id),
      Date: form.date,
      ItemId: Number(form.itemId),
      Quantity: Number(form.quantity),
      ProDate: form.proDate || null,
      ExpiryDate: form.expiryDate || null,
      PersonId: Number(form.personId),
      InventoryId: Number(form.inventoryId),
    };
    try {
      if (editing) {
        await operationsApi.update(payload);
        toast.success("Operation updated.");
      } else {
        await operationsApi.create(payload);
        toast.success("Operation added.");
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
      await operationsApi.remove(deleteTarget.id);
      toast.success("Operation deleted.");
      setDeleteTarget(null);
      load();
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  };

  const filtered = typeFilter === "All" ? operations : operations.filter((o) => o.type === typeFilter);

  const columns = [
    { key: "id", header: "ID", render: (r) => <span className="mono">{r.id}</span> },
    {
      key: "type",
      header: "Type",
      render: (r) => <span className={`badge ${typeBadgeClass(r.type)}`}>{r.type}</span>,
    },
    { key: "date", header: "Date", render: (r) => formatDate(r.date) },
    { key: "quantity", header: "Qty" },
    { key: "itemId", header: "Item", render: (r) => itemName(r.itemId) },
    { key: "personId", header: "Person", render: (r) => personName(r.personId) },
    { key: "inventoryId", header: "Inventory", render: (r) => inventoryName(r.inventoryId) },
    { key: "expiryDate", header: "Expiry", render: (r) => formatDate(r.expiryDate) },
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
        eyebrow="Permits · A-04"
        title="Operations"
        subtitle="Supply and exchange permits recorded against your inventories."
        action={
          <Button variant="accent" onClick={openCreate}>
            + Add operation
          </Button>
        }
      />

      <div className="panel">
        <div className="panel-header">
          <div className="panel-title">All operations</div>
          <Select style={{ width: 200 }} value={typeFilter} onChange={(e) => setTypeFilter(e.target.value)}>
            <option value="All">All types</option>
            {OPERATION_TYPES.map((t) => (
              <option key={t} value={t}>
                {t}
              </option>
            ))}
          </Select>
        </div>
        <DataTable columns={columns} rows={filtered} rowKey="id" loading={loading} emptyMessage="No operations recorded yet." />
      </div>

      {modalOpen && (
        <Modal
          title={editing ? "Edit operation" : "Add new operation"}
          onClose={() => setModalOpen(false)}
          footer={
            <>
              <Button variant="outline" onClick={() => setModalOpen(false)} disabled={saving}>
                Cancel
              </Button>
              <Button variant="accent" onClick={handleSubmit} loading={saving}>
                {editing ? "Save changes" : "Add operation"}
              </Button>
            </>
          }
        >
          {formError && <div className="alert alert-danger">{formError}</div>}
          <form onSubmit={handleSubmit}>
                        <Field label="Type" required>
                <Select name="type" value={form.type} onChange={handleTypeChange} required>
                <option value="">Select a type…</option>
                  {OPERATION_TYPES.map((t) => (
                    <option key={t} value={t}>
                      {t}
                    </option>
                  ))}
                </Select>
              </Field>
            <div className="field-row">
              <Field label="Operation ID" required hint={editing ? "" : "Must be unique."}>
                <Input type="number" name="id" value={form.id} onChange={handleChange} required disabled={!!editing || peopleByOperationType.length<1} />
              </Field>
              <Field label="Date" required>
                <Input type="date" name="date" value={form.date} onChange={handleChange} required disabled={peopleByOperationType.length<1} />
              </Field>
            </div>

            <div className="field-row">
              <Field label="Item" required>
              <Select name="itemId" value={form.itemId} onChange={handleChange} required disabled={peopleByOperationType.length<1}>
                <option value="">Select an item…</option>
                {items.map((it) => (
                  <option key={it.id} value={it.id}>
                    {it.name} (#{it.id})
                  </option>
                ))}
              </Select>
            </Field>
              <Field label="Quantity" required>
                <Input type="number" min={1} name="quantity" value={form.quantity} onChange={handleChange} required disabled={peopleByOperationType.length<1}/>
              </Field>
            </div>

            <div className="field-row">
              <Field label="Production date" required hint="Cannot be in the future.">
                <Input type="date" name="proDate" value={form.proDate} onChange={handleChange} required disabled={peopleByOperationType.length<1}/>
              </Field>
              <Field label="Expiry date" required hint="Must be after production date.">
                <Input type="date" name="expiryDate" value={form.expiryDate} onChange={handleChange} required disabled={peopleByOperationType.length<1}/>
              </Field>
            </div>

            <div className="field-row">
              <Field label="Person" required>
                <Select name="personId" value={form.personId} onChange={handleChange} required disabled={peopleByOperationType.length<1}>
                  <option value="">Select a person…</option>
                  {peopleByOperationType.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.name} ({p.type})
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="Inventory" required>
                <Select name="inventoryId" value={form.inventoryId} onChange={handleChange} required>
                  <option value="">Select an inventory…</option>
                  {inventories.map((inv) => (
                    <option key={inv.id} value={inv.id}>
                      {inv.name} (#{inv.id})
                    </option>
                  ))}
                </Select>
              </Field>
            </div>
          </form>
        </Modal>
      )}

      {deleteTarget && (
        <ConfirmDialog
          title="Delete operation"
          message={`Are you sure you want to delete operation #${deleteTarget.id}? This cannot be undone.`}
          loading={deleting}
          onConfirm={handleDelete}
          onClose={() => setDeleteTarget(null)}
        />
      )}
    </div>
  );
}
