export default function Button({
  children,
  variant = "primary",
  size = "md",
  loading = false,
  icon,
  className = "",
  ...rest
}) {
  const classes = ["btn", `btn-${variant}`, size === "sm" ? "btn-sm" : "", className]
    .filter(Boolean)
    .join(" ");

  return (
    <button className={classes} disabled={loading || rest.disabled} {...rest}>
      {loading ? (
        <span className={`spinner ${variant === "outline" || variant === "ghost" ? "spinner-dark" : ""}`} />
      ) : (
        icon
      )}
      {children}
    </button>
  );
}
