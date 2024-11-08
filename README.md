# Epicfy - Email Notifier

Esta API é um serviço para por enviar notificações por e-mail sempre que uma nova ideia é registrada. 
Abaixo está a documentação completa dos endpoints, estrutura de requests e responses:

---

## **Endpoints**  

### 1. **Enviar Notificação de Ideia Criada**
Este endpoint é chamado para avisar empresa via email que uma nova idea foi adicionada.

**Request**  
```http request
POST http://localhost:5000/api/v1/email-notifications/new-idea HTTP/1.1
Content-Type: application/json

{
    "targetEmail": "exemplo@dominio.com",
    "createdBy": "LuanGamepLAYS"
    "idea": {
        "title": "Nova Feature",
        "description": "Adicionar botão de compartilhamento nas redes sociais.",
        "createdAt": "2024-10-14T12:30:00Z"
    }
}
```

**Response**
- 200 OK

### 2. **Enviar Notificação de Confirmação**
Este endpoint é chamado para enviar uma confirmação de email com um link para o usuário confirmar o email.

**Request**
```http request
POST http://localhost:5000/api/v1/email-notifications/confirm-email HTTP/1.1
Content-Type: application/json

{
  "userName": "PedrinnhoGemePLAYs",
  "targetEmail": "exemplo@exemplo.com",
  "confirmationUrl": "https://epicfy.com/confirm?token=12345"
}
```

**Response**
- 200 OK