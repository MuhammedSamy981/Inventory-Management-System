import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function ProtectedRoute({ children }) {
  const { user, initializing } = useAuth();

  if (initializing) {
    return (
      <div style={{ display: "flex", height: "100vh", alignItems: "center", justifyContent: "center" }}>
        <div className="spinner spinner-dark" style={{ width: 28, height: 28 }} />
      </div>
    );
  }

  if (!user) return <Navigate to="/login" replace />;

  return children;
}
