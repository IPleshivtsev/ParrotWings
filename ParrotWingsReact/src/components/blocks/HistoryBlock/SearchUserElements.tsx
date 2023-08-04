import React, { useState, useEffect } from 'react'
import { IAutoCompleteItem, ISearchUserElementsProps } from '../../auxiliary/Interfaces'
import { _GetAllUsers } from '../../../api/ApiService'
import { ReactSearchAutocomplete } from 'react-search-autocomplete'

export function SearchUserElements({
  updateFormRecipientData,
  className,
  handleNameOnInput,
}: ISearchUserElementsProps) {
  const [searchedResults, updateSearchResults] = useState([])

  useEffect(() => {
    async function getSearchResults() {
      const searchResultsResponse: Array<IAutoCompleteItem> = await _GetAllUsers()
      updateSearchResults(searchResultsResponse.filter((x) => x.id !== localStorage.getItem('userId')))
    }
    getSearchResults()
  }, [])

  const handleOnSearch = () => {
    if (localStorage.getItem('disableSearchUserElementCheckValue') !== null) {
      localStorage.removeItem('disableSearchUserElementCheckValue')
    } else {
      updateFormRecipientData('', '')
      handleNameOnInput()
    }
  }

  const handleOnSelect = (item: IAutoCompleteItem) => {
    updateFormRecipientData(item.id, item.name)
    handleNameOnInput()
  }

  const handleOnClear = () => {
    updateFormRecipientData('', '')
  }

  const formatResult = (item: IAutoCompleteItem) => {
    return (
      <div className='result-wrapper'>
        <span className='result-span'>{item.name}</span>
      </div>
    )
  }

  return (
    <ReactSearchAutocomplete<IAutoCompleteItem>
      className={className}
      fuseOptions={{ keys: ['name'], threshold: 0.2 }}
      resultStringKeyName='name'
      maxResults={15}
      items={searchedResults}
      onSearch={handleOnSearch}
      onSelect={handleOnSelect}
      onClear={handleOnClear}
      autoFocus
      formatResult={formatResult}
      showIcon={false}
      showNoResults={false}
      placeholder='Введите имя'
      styling={{
        height: '34px',
        backgroundColor: 'white',
        boxShadow: 'none',
        clearIconMargin: '10px 8px 0 0',
        border: '',
      }}
    />
  )
}
