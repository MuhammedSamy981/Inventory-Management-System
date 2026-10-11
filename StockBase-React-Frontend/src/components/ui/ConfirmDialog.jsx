import Modal from "./Modal";
import Button from "./Button";

export default function ConfirmDialog({
  title = "Are you sure?",
  message,
  confirmLabel = "Delete",
  danger = true,
  loading = false,
  onConfirm,
  onClose,
}) {
  return (
    <Modal
      title={title}
      onClose={onClose}
      width="420px"
      footer={
        <>
          <Button variant="outline" onClick={onClose} disabled={loading}>
            Cancel
          </Button>
          <Button variant={danger ? "danger" : "primary"} onClick={onConfirm} loading={loading}>
            {confirmLabel}
          </Button>
        </>
      }
    >
      <p style={{ fontSize: 14, color: "var(--text-muted)", lineHeight: 1.6 }}>{message}</p>
    </Modal>
  );
}
