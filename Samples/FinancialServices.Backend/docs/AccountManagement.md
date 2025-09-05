# 账户管理服务

本文档描述了新开发的账户管理服务功能，包括用户注册、KYC验证和账户状态管理。

## 功能概述

### 核心功能
- **用户注册** - 创建新的用户账户
- **KYC验证** - 身份验证流程（Know Your Customer）
- **账户状态管理** - 账户状态的生命周期管理

### 账户状态
- `Pending` - 待审核（新注册状态）
- `Active` - 活跃（KYC通过后激活）
- `Suspended` - 暂停（可重新激活）
- `Closed` - 关闭（最终状态）

### KYC状态
- `NotStarted` - 未开始
- `InProgress` - 进行中（已提交材料，等待审核）
- `Approved` - 已通过
- `Rejected` - 已拒绝（可重新提交）

## API端点

### 账户管理
- `POST /api/accounts` - 创建账户
- `GET /api/accounts/{id}` - 获取账户详情
- `GET /api/accounts` - 获取账户列表（支持分页和筛选）

### KYC管理
- `POST /api/accounts/{id}/kyc/submit` - 提交KYC验证
- `POST /api/accounts/{id}/kyc/approve` - 审批通过KYC
- `POST /api/accounts/{id}/kyc/reject` - 拒绝KYC
- `GET /api/accounts/kyc/pending` - 获取待审核KYC列表

### 账户状态管理
- `POST /api/accounts/{id}/suspend` - 暂停账户
- `POST /api/accounts/{id}/activate` - 激活账户
- `POST /api/accounts/{id}/close` - 关闭账户

## 技术实现

### 架构层次
1. **领域层 (Domain)**
   - `Account` 聚合根
   - `AccountStatus` 和 `KycStatus` 枚举
   - 领域事件：`AccountCreatedDomainEvent`、`AccountKycSubmittedDomainEvent` 等

2. **基础设施层 (Infrastructure)**
   - `AccountRepository` 仓储实现
   - `AccountEntityTypeConfiguration` 实体配置

3. **应用层 (Application)**
   - **命令**：`CreateAccountCommand`、`SubmitKycVerificationCommand` 等
   - **查询**：`GetAccountQuery`、`GetAccountsQuery` 等
   - **事件处理器**：用于日志记录和通知

4. **表现层 (Presentation)**
   - FastEndpoints API端点
   - 请求/响应 DTO

### 核心业务规则
- 新注册账户默认为 `Pending` 状态，KYC为 `NotStarted`
- 只有KYC通过的账户才能激活
- 账户关闭后不能重新激活
- KYC被拒绝后可以重新提交

### 领域事件
系统会发布以下领域事件：
- `AccountCreatedDomainEvent` - 账户创建时
- `AccountKycSubmittedDomainEvent` - KYC提交时
- `AccountKycApprovedDomainEvent` - KYC通过时
- `AccountKycRejectedDomainEvent` - KYC拒绝时
- `AccountSuspendedDomainEvent` - 账户暂停时
- `AccountActivatedDomainEvent` - 账户激活时
- `AccountClosedDomainEvent` - 账户关闭时

### 数据库设计
`Accounts` 表字段：
- `Id` - 账户ID（GUID）
- `Email` - 邮箱（唯一）
- `FullName` - 全名
- `PhoneNumber` - 手机号（唯一）
- `Status` - 账户状态
- `KycStatus` - KYC状态
- `CreatedAt` - 创建时间
- `KycSubmittedAt` - KYC提交时间
- `KycApprovedAt` - KYC通过时间
- `KycRejectionReason` - KYC拒绝原因
- `IdentityDocumentType` - 身份证件类型
- `IdentityDocumentNumber` - 身份证件号码

## 测试覆盖

### 单元测试
- 账户创建逻辑
- KYC流程各状态转换
- 业务规则验证
- 异常情况处理

### 集成测试
- API端点测试
- 数据库操作测试
- 事件发布测试

## 使用示例

### 创建账户
```http
POST /api/accounts
Content-Type: application/json

{
  "email": "user@example.com",
  "fullName": "张三",
  "phoneNumber": "13800138000"
}
```

### 提交KYC验证
```http
POST /api/accounts/{accountId}/kyc/submit
Content-Type: application/json

{
  "identityDocumentType": "身份证",
  "identityDocumentNumber": "123456789012345678"
}
```

### 审批KYC
```http
POST /api/accounts/{accountId}/kyc/approve
```

## 开发规范遵循

本功能严格遵循项目的DDD开发规范：
- 使用聚合根管理业务逻辑
- 通过领域事件实现跨聚合通信
- 命令查询职责分离（CQRS）
- 强类型ID确保类型安全
- 仓储模式管理数据访问
- FastEndpoints提供API接口

## 扩展性

该服务设计具有良好的扩展性：
- 可以轻松添加新的账户状态
- 支持扩展KYC验证流程
- 事件驱动架构便于集成其他服务
- 支持多种身份验证方式
