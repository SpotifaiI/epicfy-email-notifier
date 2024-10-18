# Epicfy Email Nofifier

Esta API é um serviço para por enviar notificações por e-mail sempre que uma nova ideia é registrada. 
Abaixo está a documentação completa dos endpoints, estrutura de requests e responses:

---

## **Endpoints**  

### 1. **Enviar Notificação de Ideia Criada**  
#### **POST /api/notifications/idea-created**  
Este endpoint é chamado toda vez que uma nova ideia é adicionada para notificar a empresa por e-mail.  

**Request**  
```http
POST /api/notifications/idea
Content-Type: application/json

{
    "createdBy": LuanGamepLAYS
    "idea": {
        "title": "Nova Feature",
        "description": "Adicionar botão de compartilhamento nas redes sociais.",
        "createdAt": "2024-10-14T12:30:00Z"
    }
    "targetEmail": "exemplo@dominio.com",
}
```

**Response**
- 200 Created
