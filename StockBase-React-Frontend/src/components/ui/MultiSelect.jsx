import { Select } from "./Field";

export default function MultiSelect({ options, valueKey = "id", labelKey = "name", selected, onChange, placeholder = "Add…" }) {
  const available = options.filter((o) => !selected.includes(o[valueKey]));

  const addOne = (e) => {
    const val = e.target.value;
    if (!val) return;
    const num = Number(val);
    onChange([...selected, num]);
    e.target.value = "";
  };

  const removeOne = (id) => onChange(selected.filter((s) => s !== id));

  const labelFor = (id) => options.find((o) => o[valueKey] === id)?.[labelKey] || `#${id}`;

  return (
    <div>
      <Select onChange={addOne} defaultValue="">
        <option value="">{placeholder}</option>
        {available.map((o) => (
          <option key={o[valueKey]} value={o[valueKey]}>
            {o[labelKey]} (#{o[valueKey]})
          </option>
        ))}
      </Select>
      {selected.length > 0 && (
        <div className="multi-select-chips" style={{ marginTop: 8, border: "none", padding: 0 }}>
          {selected.map((id) => (
            <span className="chip" key={id}>
              {labelFor(id)}
              <button type="button" onClick={() => removeOne(id)} aria-label="Remove">
                ✕
              </button>
            </span>
          ))}
        </div>
      )}
    </div>
  );
}
