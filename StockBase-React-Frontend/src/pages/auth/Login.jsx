import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import AuthLayout from "./AuthLayout";
import { Field, Input } from "../../components/ui/Field";
import Button from "../../components/ui/Button";
import Modal from "../../components/ui/Modal";
import { useAuth } from "../../context/AuthContext";
import { useToast } from "../../context/ToastContext";
import { authApi } from "../../api/authApi";
import { extractErrorMessage } from "../../api/client";

export default function Login() {
  const { login } = useAuth();
  const toast = useToast();
  const navigate = useNavigate();

  const [form, setForm] = useState({ email: "", password: "" });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Email verification (dev-mode returns a token instead of sending an email)
  const [verifyState, setVerifyState] = useState(null); // { email, token }
  const [verifyCode, setVerifyCode] = useState("");
  const [verifying, setVerifying] = useState(false);

  // Forgot password
  const [showForgot, setShowForgot] = useState(false);
  const [forgotEmail, setForgotEmail] = useState("");
  const [forgotLoading, setForgotLoading] = useState(false);
  const [forgotResult, setForgotResult] = useState(null);
  const [resetForm, setResetForm] = useState({ token: "", newPassword: "" });
  const [resetLoading, setResetLoading] = useState(false);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const result = await login(form.email, form.password);
      if (result.needsVerification) {
        setVerifyState({ email: result.email, token: result.token });
        setVerifyCode(result.token || "");
      } else {
        navigate("/people", { replace: true });
      }
    } catch (err) {
      setError(extractErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  const handleVerify = async (e) => {
    e.preventDefault();
    setVerifying(true);
    try {
      await authApi.verifyEmail(verifyState.email, verifyCode);
      toast.success("Email verified. You can now sign in.");
      setVerifyState(null);
      setForm({ email: verifyState.email, password: "" });
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setVerifying(false);
    }
  };

  const handleForgotSubmit = async (e) => {
    e.preventDefault();
    setForgotLoading(true);
    setForgotResult(null);
    try {
      const { data } = await authApi.forgotPassword(forgotEmail);
      // Dev mode returns { email, token } directly; production sends an email
      if (data?.token) {
        setForgotResult({ email: data.email, token: data.token });
        setResetForm((f) => ({ ...f, token: data.token }));
      } else {
        toast.success(data?.Message || "If that account exists, a reset link was sent.");
        setShowForgot(false);
      }
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setForgotLoading(false);
    }
  };

  const handleResetSubmit = async (e) => {
    e.preventDefault();
    setResetLoading(true);
    try {
      await authApi.resetPassword(forgotResult.email, resetForm.token, resetForm.newPassword);
      toast.success("Password reset. Please sign in.");
      setShowForgot(false);
      setForgotResult(null);
      setForgotEmail("");
    } catch (err) {
      toast.error(extractErrorMessage(err));
    } finally {
      setResetLoading(false);
    }
  };

  if (verifyState) {
    return (
      <AuthLayout eyebrow="Step 2 of 2" title="Verify your email" subtitle={`We need to confirm ${verifyState.email} before you can sign in.`}>
        <form onSubmit={handleVerify}>
          <Field label="Verification code" required hint="In development mode this is filled in automatically.">
            <Input
              value={verifyCode}
              onChange={(e) => setVerifyCode(e.target.value)}
              required
              placeholder="Paste the token from your email"
            />
          </Field>
          <Button type="submit" variant="accent" className="btn-block" loading={verifying}>
            Verify email
          </Button>
        </form>
        <p className="auth-switch">
          <button className="auth-link" style={{ background: "none", border: "none" }} onClick={() => setVerifyState(null)}>
            ← Back to sign in
          </button>
        </p>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout eyebrow="Welcome back" title="Sign in to StockBase" subtitle="Enter your credentials to access your inventory workspace.">
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={handleSubmit}>
        <Field label="Email address" required>
          <Input
            type="email"
            name="email"
            value={form.email}
            onChange={handleChange}
            placeholder="you@company.com"
            required
            autoFocus
          />
        </Field>
        <Field label="Password" required>
          <Input
            type="password"
            name="password"
            value={form.password}
            onChange={handleChange}
            placeholder="••••••••"
            required
          />
        </Field>
        <div className="auth-forgot">
          <button type="button" onClick={() => setShowForgot(true)}>
            Forgot password?
          </button>
        </div>
        <Button type="submit" variant="accent" className="btn-block" loading={loading}>
          Sign in
        </Button>
      </form>
      <p className="auth-switch">
        Don't have an account? <Link to="/register">Create one</Link>
      </p>

      {showForgot && (
        <Modal
          title={forgotResult ? "Set a new password" : "Reset your password"}
          onClose={() => {
            setShowForgot(false);
            setForgotResult(null);
          }}
        >
          {!forgotResult ? (
            <form onSubmit={handleForgotSubmit}>
              <Field label="Email address" required>
                <Input
                  type="email"
                  value={forgotEmail}
                  onChange={(e) => setForgotEmail(e.target.value)}
                  required
                  autoFocus
                />
              </Field>
              <Button type="submit" variant="accent" className="btn-block" loading={forgotLoading}>
                Send reset instructions
              </Button>
            </form>
          ) : (
            <form onSubmit={handleResetSubmit}>
              <Field label="Reset token" required hint="Auto-filled in development mode.">
                <Input
                  value={resetForm.token}
                  onChange={(e) => setResetForm({ ...resetForm, token: e.target.value })}
                  required
                />
              </Field>
              <Field label="New password" required>
                <Input
                  type="password"
                  value={resetForm.newPassword}
                  onChange={(e) => setResetForm({ ...resetForm, newPassword: e.target.value })}
                  required
                />
              </Field>
              <Button type="submit" variant="accent" className="btn-block" loading={resetLoading}>
                Reset password
              </Button>
            </form>
          )}
        </Modal>
      )}
    </AuthLayout>
  );
}
