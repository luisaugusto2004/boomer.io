
const baseUrl = "https://boomerio.azurewebsites.net";

async function getRandomQuote() {
  try {
    const response = await fetch(`${baseUrl}/api/quotes/random`);
    if (!response.ok)
      throw new Error(`HTTP error! status: ${response.status}`);
    const data = await response.json();
    const output = document.getElementById("output");
    document.getElementById("quoteDisplay").innerHTML = `
            		<div style="display: flex; align-items: flex-start;">
                		<img src="${data.iconUrl}" style="width: 32px; height: auto; display: inline-block; margin-right: 8px; flex-shrink: 0;" alt="Ícone">
                		<span style="font-weight: bold; padding-left:5px;">
				${data.character}: 
				</span>
				<span style="margin-left: 8px;">
				"${data.value}"
					</span>
            		</div>`;
    const selectedData = {
	id: data.id,
  	franchise: data.franchise,
	iconUrl: data.iconUrl,
  	character: data.character,
        value: data.value
     };
    output.innerText = JSON.stringify(selectedData, null, 2);
  } catch (error) {
    document.getElementById("output").innerText =
      "Error fetching quote: " + error.message;
  }
}
window.onload = () => {
  getRandomQuote();
};
