# Car Care System 🚗

Sistema completo de gestão para lojas de estética automotiva, som, acessórios e películas.

## 🎯 Funcionalidades

- ✅ Cadastro e gerenciamento de clientes
- ✅ Sistema de agendamentos
- ✅ Controle de serviços e produtos
- ✅ Relatórios de clientes
- ✅ Sistema de pagamento
- ✅ Histórico de atendimentos

## 🏗️ Arquitetura

### Backend
- **Framework**: .NET 8
- **Padrão**: Clean Architecture
- **Princípios**: SOLID
- **ORM**: Entity Framework Core
- **Banco de Dados**: PostgreSQL

### Frontend
- **Biblioteca**: React 18
- **Linguagem**: TypeScript
- **Styling**: TailwindCSS
- **Ferramentas**: Vite, Axios

### Infraestrutura
- **Containerização**: Docker & Docker Compose
- **Banco de Dados**: PostgreSQL 15

## 📋 Requisitos

- .NET 8 SDK
- Node.js 18+
- Docker & Docker Compose
- PostgreSQL 15 (ou via Docker)

## 🚀 Como Executar

### Com Docker Compose
```bash
docker-compose up -d
```

### Manualmente

**Backend:**
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

**Frontend:**
```bash
cd frontend
npm install
npm run dev
```

## 📁 Estrutura do Projeto

```
car-care-system/
├── backend/                    # API .NET
│   ├── CarCareSystem.Api/
│   ├── CarCareSystem.Application/
│   ├── CarCareSystem.Domain/
│   ├── CarCareSystem.Infrastructure/
│   └── CarCareSystem.Tests/
├── frontend/                   # Aplicação React
│   ├── src/
│   ├── public/
│   └── package.json
├── database/                   # Scripts SQL
├── docker-compose.yml
└── README.md
```

## 🔐 Princípios SOLID Aplicados

- **S**ingle Responsibility Principle (SRP)
- **O**pen/Closed Principle (OCP)
- **L**iskov Substitution Principle (LSP)
- **I**nterface Segregation Principle (ISP)
- **D**ependency Inversion Principle (DIP)

## 📚 Clean Code

- Nomes significativos
- Funções pequenas e focadas
- Tratamento de erros adequado
- Comentários necessários apenas
- Testes unitários

## 👨‍💻 Autor

Mateus Lazzarin

## 📄 Licença

MIT
