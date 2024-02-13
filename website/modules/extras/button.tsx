
export default function Button({ text, onClick } : { text: string, onClick: any }) {
  return (
    <div>
      <button
        type="submit"
        id="submit"
        className="py-3 px-4 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-gray-200 bg-white text-black shadow-sm hover:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none"
        onClick={onClick}>
        {text}
      </button>
    </div>
  );
}