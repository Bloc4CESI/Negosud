const API_BASE_URL = "https://localhost:7281/api";

export async function getOrderClient(Id) {
  const response = await fetch(`${API_BASE_URL}/ClientOrderLine/GetClientCart/${Id}`,{
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!response.ok) {
    throw new Error('Erreur lors de la récupération des ligness commande');
  }

  return await response.json();
}