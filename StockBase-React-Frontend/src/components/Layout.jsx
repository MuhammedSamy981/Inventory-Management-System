import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import "./layout.css";

const NAV_ITEMS = [
  { to: "/people", label: "People", code: "A·01", icon: "◧" },
  { to: "/inventories", label: "Inventories", code: "A·02", icon: "▤" },
  { to: "/items", label: "Items", code: "A·03", icon: "◫" },
  { to: "/operations", label: "Operations", code: "A·04", icon: "⇄" },
  { to: "/transfers", label: "Transfers", code: "A·05", icon: "↦" },
];

export default function Layout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login", { replace: true });
  };

  const initials = user ? `${user.firstName?.[0] || ""}${user.lastName?.[0] || ""}`.toUpperCase() : "";

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="sidebar-brand">
          <div className="brand-mark">SB</div>
          <div>
            <div className="brand-name">StockBase</div>
            <div className="brand-sub">Inventory Control</div>
          </div>
        </div>

        <nav className="sidebar-nav">
          {NAV_ITEMS.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) => `sidebar-link ${isActive ? "active" : ""}`}
            >
              <span className="sidebar-icon">{item.icon}</span>
              <span className="sidebar-label">{item.label}</span>
              <span className="sidebar-code mono">{item.code}</span>
            </NavLink>
          ))}
        </nav>

        <div className="sidebar-footer">
          <div className="user-chip">
            <div className="user-avatar">{initials || "?"}</div>
            <div className="user-meta">
              <div className="user-name">
                {user?.firstName} {user?.lastName}
              </div>
              <div className="user-email">{user?.email}</div>
            </div>
          </div>
          <button className="btn btn-outline btn-block btn-sm" onClick={handleLogout}>
            Sign out
          </button>
        </div>
      </aside>

      <main className="main-area">
        <Outlet />
      </main>
    </div>
  );
}
