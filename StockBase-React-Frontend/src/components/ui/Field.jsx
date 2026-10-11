export function Field({ label, hint, error, required, children }) {
  return (
    <div className="field">
      {label && (
        <label>
          {label} {required && <span style={{ color: "var(--danger)" }}>*</span>}
        </label>
      )}
      {children}
      {hint && !error && <span className="hint">{hint}</span>}
      {error && <span className="error-text">{error}</span>}
    </div>
  );
}

export function Input({ invalid, ...rest }) {
  return <input className={`input ${invalid ? "invalid" : ""}`} {...rest} />;
}

export function Select({ invalid, children, ...rest }) {
  return (
    <select className={`select ${invalid ? "invalid" : ""}`} {...rest}>
      {children}
    </select>
  );
}

export function Textarea({ invalid, ...rest }) {
  return <textarea className={`textarea ${invalid ? "invalid" : ""}`} {...rest} />;
}
