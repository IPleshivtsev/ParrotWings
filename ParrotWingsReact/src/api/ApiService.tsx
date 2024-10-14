import { ILoginData, IRegisterData, ITransactionData, IAppealData } from '../components/auxiliary/Interfaces'

const baseUrl = 'https://localhost:7286/api/pw'

export async function _Login(loginData: ILoginData) {
  return await fetch(`${baseUrl}/Login`, {
    method: 'POST',
    body: JSON.stringify(loginData),
    headers: new Headers({ 'Content-Type': 'application/json; charset=UTF-8' }),
  })
    .then(async (response) => {
      debugger
      return AuthorizeResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      debugger
      return ErrorProcessing([errorStatus, errorMessage, 'Login'])
    })
}

export async function _Register(registerData: IRegisterData) {
  return await fetch(`${baseUrl}/Register`, {
    method: 'POST',
    body: JSON.stringify(registerData),
    headers: new Headers({ 'Content-Type': 'application/json; charset=UTF-8' }),
  })
    .then(async (response) => {
      return AuthorizeResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      return ErrorProcessing([errorStatus, errorMessage, 'Register'])
    })
}

export async function _GetCurrentUserData() {
  return await fetch(`${baseUrl}/GetCurrentUserData`, {
    method: 'GET',
    headers: new Headers({ Authorization: `Bearer ${localStorage.getItem('token')}` }),
  })
    .then(async (response) => {
      return ResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      return ErrorProcessing([errorStatus, errorMessage, 'GetCurrentUserData'])
    })
}

export async function _GetAllUsers() {
  CheckToken()
  return await fetch(`${baseUrl}/GetAllUsers`, {
    method: 'GET',
    headers: new Headers({ Authorization: `Bearer ${localStorage.getItem('token')}` }),
  })
    .then(async (response) => {
      return ResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      return ErrorProcessing([errorStatus, errorMessage, 'GetAllUsers'])
    })
}

export async function _CreateTransaction(transactionData: ITransactionData) {
  CheckToken()
  return await fetch(`${baseUrl}/CreateTransaction`, {
    method: 'POST',
    body: JSON.stringify(transactionData),
    headers: new Headers({
      'Content-Type': 'application/json; charset=UTF-8',
      Authorization: `Bearer ${localStorage.getItem('token')}`,
    }),
  })
    .then(async (response) => {
      return ResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      return ErrorProcessing([errorStatus, errorMessage, 'CreateTransaction'])
    })
}

export async function _GetUserTransactions() {
  CheckToken()
  return await fetch(`${baseUrl}/GetUserTransactions`, {
    method: 'GET',
    headers: new Headers({ Authorization: `Bearer ${localStorage.getItem('token')}` }),
  })
    .then(async (response) => {
      return ResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      return ErrorProcessing([errorStatus, errorMessage, 'GetUserTransactions'])
    })
}

export async function _CreateAppeal(appealData: IAppealData) {
  return await fetch(`${baseUrl}/CreateAppeal`, {
    method: 'POST',
    body: JSON.stringify(appealData),
    headers: new Headers({ 'Content-Type': 'application/json; charset=UTF-8' }),
  })
    .then(async (response) => {
      return ResponseProcessing(response)
    })
    .catch(([errorStatus, errorMessage]) => {
      return ErrorProcessing([errorStatus, errorMessage, 'CreateAppeal'])
    })
}

const AuthorizeResponseProcessing = async (response: any) => {
  try {
    const data = await response.json()
    if (response.ok) {
      if (data === '') {
        return Promise.reject([response.status, 'Данные отсутствуют'])
      }
      
      localStorage.setItem('token', data)
      const expirationDateTime = new Date()
      localStorage.setItem('expirationDateTime', new Date(expirationDateTime.getTime() + 20 * 60000).toUTCString())
      return Promise.resolve(true)
    }
    return Promise.reject([response.status, GetDataInfo(data)])
  } catch (exception) {
    if (response.status === 401) {
      return Promise.reject([response.status, 'Ошибка авторизации'])
    }
    return Promise.reject([response.status, exception])
  }
}

const ResponseProcessing = async (response: any) => {
  try {
    const data = await response.json()
    if (response.ok) {
      if (data === '') {
        return Promise.reject([response.status, 'Данные отсутствуют'])
      }
      return Promise.resolve(data)
    }
    return Promise.reject([response.status, GetDataInfo(data)])
  } catch (exception) {
    if (response.status === 401) {
      return Promise.reject([response.status, 'Ошибка авторизации'])
    }
    return Promise.reject([response.status, exception])
  }
}

const ErrorProcessing = ([errorStatus, errorMessage, methodName]: Array<string>) => {
  if (errorStatus === undefined && errorMessage === undefined) {
    errorStatus = '503'
    errorMessage = 'Сервис недоступен. Попробуйте позже'
  }

  console.log(`ApiService.${methodName} | ${errorStatus}: ${errorMessage}`)
  return errorMessage
}

const CheckToken = () => {
  const expirationDateTime = new Date(localStorage.getItem('expirationDateTime'))
  const currentDateTime = new Date()

  if (expirationDateTime < currentDateTime) {
    localStorage.clear()
    window.location.href = '/loginUser'
  }
}

const GetDataInfo = (data: any) => {
  return typeof data === 'string'
    ? data
    : data.title !== undefined
      ? `${data.title}: ${JSON.stringify(data.errors)}`
      : JSON.stringify(data)
}
