import { useEffect, useState } from "react";
import PageHeader, { randomId, formatDate } from "../components/ui/PageHeader";
import Button from "../components/ui/Button";
import DataTable from "../components/ui/DataTable";
import Modal from "../components/ui/Modal";
import { Field, Input, Select } from "../components/ui/Field";
import { operationsApi } from "../api/operationsApi";
import { itemsApi } from "../api/itemsApi";
import { inventoriesApi } from "../api/inventoriesApi";
import { extractErrorMessage } from "../api/client";
import { useToast } from "../context/ToastContext";

const TRANSFER_PREFIX = "Transfer from InventoryId=";

const emptyForm = {
  id: "",
  fromInventoryId: "",
  toInventoryId: "",
  itemId: "",
  quantity: "",
};

function parseFromInventoryId(type) {
  if (!type || !type.startsWith(TRANSFER_PREFIX)) return null;
  const raw = type.slice(TRANSFER_PREFIX.length);
  const num = Number(raw);
  return Number.isNaN(num) ? raw : num;
}

export default function Transfers() {
  const toast = useToast();
  const [operations, setOperations] = useState([]);
  const [items, setItems] = useState([]);
 // const [people, setPeople] = useState([]);
  const [inventories, setInventories] = useState([]);
  const [loading, setLoading] = useState(true);

  const [modalOpen, setModalOpen] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState("");
  const [formInfo, setFormInfo] = useState("");

  const load = async () => {
    setLoading(true);
    try {
      const [opsRes, itemsRes, invRes] = await Promise.all([
        operationsApi.getByType(TRANSFER_PREFIX, 1, 200),
        itemsApi.getAll(1, 200),
        inventoriesApi.getAll(1, 200),
      ]);
      //console.log("opsRes", opsRes);
      setOperations(opsRes.data || []);
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

  const itemName = (id) => items.find((i) => i.id === id)?.name || `#${id}`;

  const inventoryName = (id) => inventories.find((i) => i.id === id)?.name || `#${id}`;

  const transfers = operations
    .filter((o) => o.type && o.type.startsWith(TRANSFER_PREFIX))
    .map((o) => ({ ...o, fromInventoryId: parseFromInventoryId(o.type) }));
    
  const openCreate = () => {
    setForm({ ...emptyForm, id: randomId() });
    setFormError("");
    setFormInfo("");
    setModalOpen(true);
  };

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormError("");
    setFormInfo("");
    setSaving(true);
    const payload = {
      Id: Number(form.id),
      FromInventoryId: Number(form.fromInventoryId),
      ToInventoryId: Number(form.toInventoryId),
      ItemId: Number(form.itemId),
      Quantity: Number(form.quantity),
    };
    try {
      const { data } = await operationsApi.createTransfer(payload);
      const message = data?.Message || data?.message || "";
      if (message && /fail|more than|insert quantity/i.test(message)) {
        setFormError(message);
      } else {
        toast.success(message || "Transfer created.");
        setModalOpen(false);
        load();
      }
    } catch (err) {
      setFormError(extractErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  const columns = [
    { key: "id", header: "ID", render: (r) => <span className="mono">{r.id}</span> },
    { key: "fromInventoryId", header: "From", render: (r) => inventoryName(r.fromInventoryId) },
    { key: "inventoryId", header: "To", render: (r) => inventoryName(r.inventoryId) },
    { key: "itemId", header: "Item", render: (r) => itemName(r.itemId) },
    { key: "quantity", header: "Qty" },
    { key: "date", header: "Date", render: (r) => formatDate(r.date) },
  ];

  return (
    <div>
      <PageHeader
        eyebrow="Movements · A-05"
        title="Transfers"
        subtitle="Stock moved between inventories. Transfers are recorded and cannot be edited or deleted."
        action={
          <Button variant="accent" onClick={openCreate}>
            + Add transfer
          </Button>
        }
      />

      <div className="panel">
        <div className="panel-header">
          <div className="panel-title">All transfers</div>
        </div>
        <DataTable
          columns={columns}
          rows={transfers}
          rowKey="id"
          loading={loading}
          emptyMessage="No transfers recorded yet — move stock between inventories to see it here."
        />
      </div>

      {modalOpen && (
        <Modal
          title="Add new transfer"
          onClose={() => setModalOpen(false)}
          footer={
            <>
              <Button variant="outline" onClick={() => setModalOpen(false)} disabled={saving}>
                Cancel
              </Button>
              <Button variant="accent" onClick={handleSubmit} loading={saving}>
                Create transfer
              </Button>
            </>
          }
        >
          {formError && <div className="alert alert-danger">{formError}</div>}
          {formInfo && <div className="alert alert-info">{formInfo}</div>}
          <form onSubmit={handleSubmit}>
            <Field label="Transfer ID" required hint="Must be unique.">
              <Input type="number" name="id" value={form.id} onChange={handleChange} required />
            </Field>
            <div className="field-row">
              <Field label="From inventory" required>
                <Select name="fromInventoryId" value={form.fromInventoryId} onChange={handleChange} required>
                  <option value="">Select…</option>
                  {inventories.map((inv) => (
                    <option key={inv.id} value={inv.id}>
                      {inv.name} (#{inv.id})
                    </option>
                  ))}
                </Select>
              </Field>
              <Field label="To inventory" required>
                <Select name="toInventoryId" value={form.toInventoryId} onChange={handleChange} required>
                  <option value="">Select…</option>
                  {inventories.map((inv) => (
                    <option key={inv.id} value={inv.id}>
                      {inv.name} (#{inv.id})
                    </option>
                  ))}
                </Select>
              </Field>
            </div>
            <Field label="Item" required>
              <Select name="itemId" value={form.itemId} onChange={handleChange} required>
                <option value="">Select an item…</option>
                {items.map((it) => (
                  <option key={it.id} value={it.id}>
                    {it.name} (#{it.id})
                  </option>
                ))}
              </Select>
            </Field>
              <Field label="Quantity" required>
                <Input type="number" min={1} name="quantity" value={form.quantity} onChange={handleChange} required />
              </Field>
          </form>
        </Modal>
      )}
    </div>
  );
}
