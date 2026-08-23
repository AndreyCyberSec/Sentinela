# 🛡️ Sentinela

> **Sentinela** é uma solução de monitoramento de segurança e análise de logs orientada à detecção de ameaças e integridade de infraestrutura.

---

## 📌 Sobrevistas e Objetivos

O projeto **Sentinela** foi desenvolvido para centralizar, analisar e correlacionar eventos de segurança em ambientes de TI. A aplicação atua identificando comportamentos anômalos, falhas de autenticação e potenciais vetores de ataque em tempo real, auxiliando em rotinas de resposta a incidentes.

### Principais Recursos
* **Análise de Logs em Tempo Real:** Leitura e processamento contínuo de registros de eventos do sistema.
* **Detecção de Anomalias:** Regras de identificação para padrões suspeitos (ex: ataques de força bruta, acessos não autorizados).
* **Estrutura Modular:** Integração simples com agentes de segurança e plataformas de SIEM (como Wazuh).
* **Alertas e Relatórios:** Geração de relatórios de auditoria e notificações de incidentes.

---

## 📁 Diretórios e estrutura de arquivos do sistema
```text
ScanLog/
│
├── 📁 Application/
│   └── 📁 Service/
│
├── 📁 Core/
│   ├── 📁 Interface/
│   │   └── IFileReader.cs
│   └── 📁 Models/
│       ├── JsonEntity.cs
│       └── LogEntity.cs
│
└── 📁 Sentinela/
    └── Scanner.cs
```

### 📌 Descrição dos Módulos
* **Application:** Camada de orquestração e regras de aplicação (Service).
* **Core:** Núcleo da solução, contendo as interfaces (Interface) e as entidades de dados (Models).
* **Sentinela:** Projeto de execução/monitoramento responsável pelo escaneamento (Scanner.cs).

---

## 📝 Arquitetura 
* **SOLID:** Utilizado o princípio SOLID para modelar a arquitetura do sistema e tornar de toda estrutura do desenvolvimento mais limpo e com facilidade para refatoração do código, implementação de novas funcionalidades e desacoplamento de classes.

## 🛠️ Tecnologias Utilizadas

* **Linguagem / Runtime:** C# / .NET (ou Python/Bash conforme a implementação)
* **Gerenciamento de Logs & SIEM:** Wazuh / Syslog
* **Controle de Versão:** Git & GitHub

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* Git instalado na máquina
* SDK do .NET (ou runtime correspondente à sua stack)
* Permissões de administrador/root para leitura de logs do sistema

### Passo a Passo

1. **Clonar o Repositório:**
   ```bash
   git clone [https://github.com/seu-usuario/sentinela.git](https://github.com/seu-usuario/sentinela.git)
   cd sentinela