import React, { useState, useEffect } from 'react'
import './HistoryBlock.less'
import { useLocation, useNavigate } from 'react-router-dom'
import { Image } from 'react-bootstrap'
import DataTable from 'react-data-table-component'
import { IHistoryBlockProps, IPresentTransactionData, ITransactionData } from '../../auxiliary/Interfaces'
import { _GetUserTransactions } from '../../../api/ApiService'
import { customSort, customStyles, paginationComponentOptions } from './DataTableOptions'

export function HistoryBlock({ setIsShowHistoryBlock }: IHistoryBlockProps) {
  const location = useLocation()
  const navigate = useNavigate()
  const [tableData, setTableData] = useState([])

  useEffect(() => {
    async function getUserTransactions() {
      const transactionsData: Array<ITransactionData> = await _GetUserTransactions()
      const presentTransactionData = new Array<IPresentTransactionData>()

      transactionsData.forEach((element) => {
        const isReplenishment = element.TransferCardNumber != ''
        presentTransactionData.push({
          Id: element.Id,
          CreatedDateString: new Date(element.CreatedDate).toLocaleString(),
          CreatedDate: element.CreatedDate,
          SenderId: element.SenderId,
          SenderName: isReplenishment
            ? `**** ${element.TransferCardNumber.replace(/[^\d]/g, '').substring(12, 16)}`
            : element.SenderName,
          RecipientId: element.RecipientId,
          RecipientName: element.RecipientName,
          Amount: element.Amount,
        })
      })
      setTableData(presentTransactionData)
    }
    getUserTransactions()
  }, [])

  const handleRepeatTransfer = (e: any) => {
    const row = e.currentTarget.parentElement.closest("[role='row']")
    const repeatRecipientId = row.querySelector("[data-column-id='3']").children[0].getAttribute('recipient-id')
    const repeatRecipientName = row.querySelector("[data-column-id='3']").children[0].getAttribute('recipient-name')
    const repeatAmount = row.querySelector("[data-column-id='4']").children[0].innerHTML.replace('-', '')
    localStorage.setItem('repeatRecipientId', repeatRecipientId)
    localStorage.setItem('repeatRecipientName', repeatRecipientName)
    localStorage.setItem('repeatAmount', repeatAmount)

    location.pathname === '/transfer' 
      ? setIsShowHistoryBlock(false)
      : navigate('/transfer')
  }

  return (
    <>
      <DataTable
        columns={[
          {
            name: 'Дата операции',
            selector: (row: any) => row.CreatedDateString,
            sortable: true,
            minWidth: '165px',
          },
          {
            name: 'Отправитель',
            selector: (row: any) => row.SenderName,
            sortable: true,
            minWidth: '135px',
          },
          {
            name: 'Получатель',
            selector: (row: any) => row.RecipientName,
            cell: (row: any) => {
              return (
                <span recipient-id={row.RecipientId} recipient-name={row.RecipientName}>
                  {row.RecipientName}
                </span>
              )
            },
            sortable: true,
            minWidth: '135px',
          },
          {
            name: 'Сумма',
            selector: (row: any) => row.Amount,
            cell: (row: any) => {
              return row.SenderName.includes('****') || row.RecipientName === localStorage.getItem('userName') ? (
                <span style={{ color: 'green' }}>{row.Amount}</span>
              ) : (
                <span style={{ color: 'red' }}>{`-${row.Amount}`}</span>
              )
            },
            sortable: true,
            minWidth: '75px',
          },
          {
            cell: (row: any) => {
              return !row.SenderName.includes('****') && row.RecipientName !== localStorage.getItem('userName') ? (
                <Image src='/images/repeat.svg' onClick={handleRepeatTransfer} />
              ) : (
                <Image className={'hidden'} src='/images/repeat.svg' onClick={handleRepeatTransfer} />
              )
            },
            button: true,
            maxWidth: '100%',
            minWidth: '-1px',
            style: { cursor: 'pointer' },
          },
        ]}
        data={tableData}
        persistTableHead
        pagination
        sortFunction={customSort}
        defaultSortAsc={false}
        defaultSortFieldId={1}
        paginationComponentOptions={paginationComponentOptions}
        customStyles={customStyles}
        noDataComponent={<div>Операции не найдены</div>}
      />
    </>
  )
}
