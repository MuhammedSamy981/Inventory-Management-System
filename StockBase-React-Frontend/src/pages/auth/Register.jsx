import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import AuthLayout from "./AuthLayout";
import { Field, Input } from "../../components/ui/Field";
import Button from "../../components/ui/Button";
import { useAuth } from "../../context/AuthContext";
import { extractErrorMessage } from "../../api/client";

const initialForm = {
  firstName: "",
  lastName: "",
  email: "",
  password: "",
  phoneNumber: "",
  amount: 30,
  currency: "$",
};

export default function Register() {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [form, setForm] = useState(initialForm);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [done, setDone] = useState(null);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((f) => ({ ...f, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const payload = {
        FirstName: form.firstName,
        LastName: form.lastName,
        Email: form.email,
        Password: form.password,
        PhoneNumber: form.phoneNumber,
        Amount: Number(form.amount) || 0,
        Currency: form.currency,
      };
      const result = await register(payload);
      setDone(result?.Message || result?.message || "Account created successfully. You can now sign in.");
    } catch (err) {
      setError(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  if (done) {
    return (
      <AuthLayout eyebrow="Account created" title="Check your inbox" subtitle={done}>
        <Button variant="accent" className="btn-block" onClick={() => navigate("/login")}>
          Go to sign in
        </Button>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout
      eyebrow="Get started"
      title="Create your account"
      subtitle="Set up a StockBase account to start managing your inventory."
    >
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={handleSubmit}>
        <div className="field-row">
          <Field label="First name" required>
            <Input name="firstName" value={form.firstName} onChange={handleChange} required autoFocus />
          </Field>
          <Field label="Last name" required>
            <Input name="lastName" value={form.lastName} onChange={handleChange} required />
          </Field>
        </div>
        <Field label="Email address" required>
          <Input type="email" name="email" value={form.email} onChange={handleChange} required />
        </Field>
        <Field label="Phone number" required>
          <Input name="phoneNumber" value={form.phoneNumber} onChange={handleChange} required />
        </Field>
        <Field
          label="Password"
          required
          hint="At least 8 characters, with upper &amp; lower case, a digit and a symbol."
        >
          <Input type="password" name="password" value={form.password} onChange={handleChange} required minLength={8} />
        </Field>
        <Button type="submit" variant="accent" className="btn-block" loading={loading}>
          Create account
        </Button>
      </form>
      <p className="auth-switch">
        Already have an account? <Link to="/login">Sign in</Link>
      </p>
    </AuthLayout>
  );
}
