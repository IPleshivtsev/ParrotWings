export const paginationComponentOptions = { rowsPerPageText: 'Строк на странице', rangeSeparatorText: 'из' }

export const customStyles = {
  headRow: {
    style: {
      border: 'none',
    },
  },
  headCells: {
    style: {
      color: '#202124',
      fontSize: '14px',
    },
  },
  rows: {
    highlightOnHoverStyle: {
      backgroundColor: 'rgb(230, 244, 244)',
      borderBottomColor: '#FFFFFF',
      borderRadius: '25px',
      outline: '1px solid #FFFFFF',
    },
  },
  pagination: {
    style: {
      border: 'none',
    },
  },
}

export const customSort = (rows: any, selector: any, direction: any) => {
  return rows.sort((a: any, b: any) => {
    const isDate = selector(a) === a.CreatedDateString
    const isInt = selector(a) === a.Amount

    const aField = isDate ? a.CreatedDate : isInt ? a.Amount : selector(a).toLowerCase()
    const bField = isDate ? b.CreatedDate : isInt ? b.Amount : selector(b).toLowerCase()

    let comparison = 0

    if (aField > bField) {
      comparison = 1
    } else if (aField < bField) {
      comparison = -1
    }
    return direction === 'desc' ? comparison * -1 : comparison
  })
}
