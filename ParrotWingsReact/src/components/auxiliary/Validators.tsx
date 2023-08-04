//#region Consts
export const functionalAndNumberKeysArray = [8, 37, 38, 39, 40, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57]
export const functionalKeysArray = [8, 37, 38, 39, 40, 46]
//#endregion

//#region OnKeyDownHandles
export const handleAmountOnKeyDown = async (e: any) => {
  if (!functionalAndNumberKeysArray.includes(e.keyCode) || (e.target.value.length === 0 && e.keyCode === 48)) {
    e.preventDefault()
  }
}

export const handleStringOnKeyDown = async (e: any, regExp: RegExp, length: number) => {
  if (
    !functionalAndNumberKeysArray.includes(e.keyCode) ||
    (e.target.value.replace(regExp, '').length === length && !functionalKeysArray.includes(e.keyCode))
  ) {
    e.preventDefault()
  }
}
//#endregion

//#region Functions
export function validateText(value: string) {
  return checkNullValue(value)
}

export function validateEmail(value: string) {
  const lastAtPos = value.lastIndexOf('@')
  const lastDotPos = value.lastIndexOf('.')

  let result = checkNullValue(value)

  return result !== ''
    ? result
    : !(
        lastAtPos < lastDotPos &&
        lastAtPos > 0 &&
        value.indexOf('@@') == -1 &&
        lastDotPos > 2 &&
        value.length - lastDotPos > 2
      )
    ? 'Email некорректен'
    : ''
}

export function validatePassword(value: string) {
  let result = checkNullValue(value)
  return result !== '' ? result : value.length < 6 ? 'Длина пароля не может быть меньше 6-ти символов' : ''
}

export function validateConfirmPassword(value: string, confirmValue: string) {
  let result = checkNullValue(value)

  return result !== '' ? result : value !== confirmValue ? 'Значения паролей не совпадают' : ''
}

export function validateAmount(value: string) {
  let result = checkNullValue(value)

  return result !== ''
    ? result
    : value[0] === '0'
    ? 'Значение не может начинаться с нуля'
    : checkIncorrectValue(value, /\D+/g)
}

export function validateCardNumber(value: string) {
  value = value.replace(/\s|_/g, '')

  let result = checkNullValue(value)
  if (result !== '') {
    return result
  }

  result = checkValueLength(value, 16)
  return result !== '' ? result : checkIncorrectValue(value, /\D+/g)
}

export function validateCardDate(value: string) {
  value = value.replace(/\/|_/g, '')

  let result = checkNullValue(value)
  if (result !== '') {
    return result
  }

  result = checkValueLength(value, 4)
  if (result !== '') {
    return result
  }

  result = checkIncorrectValue(value, /\D+/g)
  return result !== ''
    ? result
    : value.substring(0, 2) === '00' ||
      parseInt(value.substring(0, 2)) > 12 ||
      parseInt(value.substring(2, 4)) < parseInt(new Date().getFullYear().toString().substring(2, 4))
    ? 'Некорректная дата'
    : ''
}

export function validateCardCVV(value: string) {
  value = value.replace(/_/g, '')

  let result = checkNullValue(value)
  if (result !== '') {
    return result
  }

  result = checkValueLength(value, 3)
  return result !== '' ? result : checkIncorrectValue(value, /\D+/g)
}
//#endregion

//#region Helpers
const checkNullValue = (value: string) => {
  return value === '' || value === null || value === undefined ? 'Значение не может быть пустым' : ''
}

const checkValueLength = (value: string, length: number) => {
  return value.length !== length
    ? `Значение должно иметь ${length} ${[3, 4].includes(length) ? 'символа' : 'символов'}`
    : ''
}

const checkIncorrectValue = (value: string, regExp: RegExp) => {
  return regExp.test(value) ? 'В значении присутствуют недопустимые символы' : ''
}
//#endregion
