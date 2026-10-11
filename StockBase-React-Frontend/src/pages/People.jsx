import { useEffect, useState } from "react";
import PageHeader, { randomId } from "../components/ui/PageHeader";
import Button from "../components/ui/Button";
import DataTable from "../components/ui/DataTable";
import Modal from "../components/ui/Modal";
import ConfirmDialog from "../components/ui/ConfirmDialog";
import { Field, Input, Select } from "../components/ui/Field";
import { peopleApi } from "../api/peopleApi";
import { extractErrorMessage } from "../api/client";
import { useToast } from "../context/ToastContext";

const PERSON_TYPES = ["Inventory Manager", "Supplier", "Client"];

const emptyForm = {
  id: "",
  name: "",
  telephone: "",
  fax: "",
  phone: "",
  email: "",
  website: "",
  type: "Inventory Manager",
};

function typeBadgeClass(type) {
  if (type === "Inventory Manager") return "badge-manager";
  if (type === "Supplier") return "badge-supplier";
  if (type === "Client") return "badge-client";
  return "badge-neutral";
}

export default function People() {
  const toast = useToast();
  const [people, setPeople] = useState([]);
  const [loading, setLoading] = useState(true);
  const [typeFilter, setTypeFilter] = useState("All");

  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [formError, setFormError] = useState("");

  const [deleteTarget, setDeleteTarget] = useState(null);
  const [deleting, setDeleting] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await peopleApi.getAll(1, 200);
      setPeople(data || []);
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

  const openEdit = (person) => {
    setEditing(person);
    setForm({
      id: person.id,
      name: person.name || "",
      telephone: person.telephone || "",
      fax: person.fax || "",
      phone: person.phone || "",
      email: person.email || "",
      website: person.website || "",
      type: person.type || "Inventory Manager",
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
      Telephone: form.telephone,
      Fax: form.fax,
      Phone: form.phone,
      Email: form.email,
      Website: form.website,
      Type: form.type,
    };
    try {
      if (editing) {
        await peopleApi.update(payload);
        toast.success("Person updated.");
      } else {
        await peopleApi.create(payload);
        toast.success("Person added.");
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
      await peopleApi.remove(deleteTarget.id);
      toast.success("Person deleted.");
      setDeleteTarget(null);
      load();
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setDeleting(false);
    }
  };

  const filtered = typeFilter === "All" ? people : people.filter((p) => p.type === typeFilter);

  const columns = [
    { key: "id", header: "ID", render: (r) => <span className="mono">{r.id}</span> },
    { key: "name", header: "Name" },
    {
      key: "type",
      header: "Type",
      render: (r) => <span className={`badge ${typeBadgeClass(r.type)}`}>{r.type}</span>,
    },
    { key: "phone", header: "Phone", render: (r) => r.phone || "—" },
    { key: "email", header: "Email", render: (r) => r.email || "—" },
    { key: "website", header: "Website", render: (r) => r.website || "—" },
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
        eyebrow="Directory · A-01"
        title="People"
        subtitle="Inventory managers, suppliers and clients tied to your account."
        action={
          <Button variant="accent" onClick={openCreate}>
            + Add person
          </Button>
        }
      />

      <div className="panel">
        <div className="panel-header">
          <div className="panel-title">All people</div>
          <Select style={{ width: 200 }} value={typeFilter} onChange={(e) => setTypeFilter(e.target.value)}>
            <option value="All">All types</option>
            {PERSON_TYPES.map((t) => (
              <option key={t} value={t}>
                {t}
              </option>
            ))}
          </Select>
        </div>
        <DataTable columns={columns} rows={filtered} rowKey="id" loading={loading} emptyMessage="No people added yet — add your first person to get started." />
      </div>

      {modalOpen && (
        <Modal
          title={editing ? "Edit person" : "Add new person"}
          onClose={() => setModalOpen(false)}
          footer={
            <>
              <Button variant="outline" onClick={() => setModalOpen(false)} disabled={saving}>
                Cancel
              </Button>
              <Button variant="accent" onClick={handleSubmit} loading={saving}>
                {editing ? "Save changes" : "Add person"}
              </Button>
            </>
          }
        >
          {formError && <div className="alert alert-danger">{formError}</div>}
          <form onSubmit={handleSubmit}>
            <div className="field-row">
              <Field label="Person ID" required hint={editing ? "" : "Must be unique."}>
                <Input
                  type="number"
                  name="id"
                  value={form.id}
                  onChange={handleChange}
                  required
                  disabled={!!editing}
                />
              </Field>
              <Field label="Type" required>
                <Select name="type" value={form.type} onChange={handleChange} required>
                  {PERSON_TYPES.map((t) => (
                    <option key={t} value={t}>
                      {t}
                    </option>
                  ))}
                </Select>
              </Field>
            </div>
            <Field label="Full name" required>
              <Input name="name" value={form.name} onChange={handleChange} required minLength={2} maxLength={20} />
            </Field>
            <div className="field-row">
              <Field label="Telephone" required>
                <Input name="telephone" value={form.telephone} onChange={handleChange} required />
              </Field>
              <Field label="Phone">
                <Input name="phone" value={form.phone} onChange={handleChange} />
              </Field>
            </div>
            <div className="field-row">
              <Field label="Fax">
                <Input name="fax" value={form.fax} onChange={handleChange} />
              </Field>
              <Field label="Email" required>
                <Input type="email" name="email" value={form.email} onChange={handleChange} required />
              </Field>
            </div>
            <Field label="Website" hint="Must contain a '.' — e.g. company.com">
              <Input name="website" value={form.website} onChange={handleChange} />
            </Field>
          </form>
        </Modal>
      )}

      {deleteTarget && (
        <ConfirmDialog
          title="Delete person"
          message={`Are you sure you want to delete "${deleteTarget.name}"? This cannot be undone.`}
          loading={deleting}
          onConfirm={handleDelete}
          onClose={() => setDeleteTarget(null)}
        />
      )}
    </div>
  );
}
