
const baseUrl = "https://boomerio.azurewebsites.net";

function escapeHtml(s) {
  return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
}

function syntaxHighlight(obj) {
  let json = typeof obj === "string" ? obj : JSON.stringify(obj, null, 2);
  json = escapeHtml(json);
  
  const tokenRegex =
    /("([^\\"]|\\.)*"(?=\s*:))|("([^\\"]|\\.)*")|\btrue\b|\bfalse\b|\bnull\b|-?\d+(?:\.\d+)?(?:[eE][+\-]?\d+)?|([\{\}\[\]\:,])/g;

  return json.replace(tokenRegex, (match, p1, _p2, p3, _p4, p5) => {
    if (p1) {
      return `<span class="key">${p1}</span>`;
    }
    if (p3) {
      const isUrl = /^"(https?:\/\/[^"]+)"$/.test(p3);
      return `<span class="string value${isUrl ? " url" : ""}">${p3}</span>`;
    }
    if (match === "true" || match === "false") {
      return `<span class="boolean">${match}</span>`;
    }
    if (match === "null") {
      return `<span class="null">${match}</span>`;
    }
    if (p5) {
      return `<span class="signal">${p5}</span>`;
    }
    return `<span class="number">${match}</span>`;
  });
}

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
    output.innerHTML = syntaxHighlight(data);
  } catch (error) {
    document.getElementById("output").innerText =
      "Error fetching quote: " + error.message;
  }
}
window.onload = () => {
  getRandomQuote();
};
