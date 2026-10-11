import "./auth.css";

export default function AuthLayout({ children, eyebrow, title, subtitle }) {
  return (
    <div className="auth-shell">
      <div className="auth-brand-panel">
        <div className="auth-brand-content">
          <div className="auth-brand-mark">SB</div>
          <h1>StockBase</h1>
          <p>Warehouse-grade inventory control for people, stock, permits and transfers — all in one place.</p>

          <div className="auth-feature-list">
            <div className="auth-feature">
              <span className="auth-feature-icon">◧</span>
              <span>Track suppliers, clients &amp; inventory managers</span>
            </div>
            <div className="auth-feature">
              <span className="auth-feature-icon">▤</span>
              <span>Run storage, expiry &amp; movement reports on demand</span>
            </div>
            <div className="auth-feature">
              <span className="auth-feature-icon">⇄</span>
              <span>Issue supply and exchange permits with a full audit trail</span>
            </div>
          </div>
        </div>
        <div className="auth-grid-decoration" aria-hidden="true" />
      </div>

      <div className="auth-form-panel">
        <div className="auth-form-card">
          {eyebrow && <span className="page-eyebrow">{eyebrow}</span>}
          <h2>{title}</h2>
          {subtitle && <p className="auth-subtitle">{subtitle}</p>}
          {children}
        </div>
      </div>
    </div>
  );
}
