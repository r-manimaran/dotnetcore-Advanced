## AI Completion using .Net and Ollama

```bash
docker run -d -v ollama_data:/root/.ollama -p 11434:11434 --name ollama ollama/ollama:latest

 docker exec -it ollama ollama pull llama3
```