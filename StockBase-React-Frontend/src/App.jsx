import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { AuthProvider, useAuth } from "./context/AuthContext";
import { ToastProvider } from "./context/ToastContext";
import ProtectedRoute from "./components/ProtectedRoute";
import Layout from "./components/Layout";
import Login from "./pages/auth/Login";
import Register from "./pages/auth/Register";
import People from "./pages/People";
import Inventories from "./pages/Inventories";
import Items from "./pages/Items";
import Operations from "./pages/Operations";
import Transfers from "./pages/Transfers";

function RootRedirect() {
  const { user, initializing } = useAuth();
  if (initializing) return null;
  return <Navigate to={user ? "/people" : "/login"} replace />;
}

function PublicOnlyRoute({ children }) {
  const { user, initializing } = useAuth();
  if (initializing) return null;
  if (user) return <Navigate to="/people" replace />;
  return children;
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <ToastProvider>
          <Routes>
            <Route path="/" element={<RootRedirect />} />

            <Route
              path="/login"
              element={
                <PublicOnlyRoute>
                  <Login />
                </PublicOnlyRoute>
              }
            />
            <Route
              path="/register"
              element={
                <PublicOnlyRoute>
                  <Register />
                </PublicOnlyRoute>
              }
            />

            <Route
              element={
                <ProtectedRoute>
                  <Layout />
                </ProtectedRoute>
              }
            >
              <Route path="/people" element={<People />} />
              <Route path="/inventories" element={<Inventories />} />
              <Route path="/items" element={<Items />} />
              <Route path="/operations" element={<Operations />} />
              <Route path="/transfers" element={<Transfers />} />
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </ToastProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}
