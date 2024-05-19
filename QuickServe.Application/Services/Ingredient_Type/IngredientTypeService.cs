using AutoMapper;
using QuickServe.Application.Interfaces.IngredientTypes;
using QuickServe.Application.Interfaces;
using QuickServe.Application.ViewModels.IngredientType;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickServe.Domain.Entities;
using QuickServe.Domain.Enums;

namespace QuickServe.Application.Services.Ingredient_Type
{
    public class IngredientTypeService : IIngredientTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IngredientTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IngredientTypeDTO>> CreateIngredientTypeAsync(AddUpdateIngredientTypeDTO IngredientTypeDto, int userId)
        {
            var response = new ServiceResponse<IngredientTypeDTO>();
            try
            {
                var now = DateTime.Now;
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                var IngredientType = _mapper.Map<IngredientType>(IngredientTypeDto);
                IngredientType.CreatedDate = now;
                IngredientType.UpdatedDate = now;
                IngredientType.CreatedBy = user.Username;
                IngredientType.UpdatedBy = user.Username;
                IngredientType.Status = UserStatus.Active.ToString();
                await _unitOfWork.IngredientTypeRepository.AddAsync(IngredientType);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    var IngredientTypeDTO = _mapper.Map<IngredientTypeDTO>(IngredientType);
                    response.Data = IngredientTypeDTO;
                    response.Success = true;
                    response.Message = "IngredientType created successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create IngredientType failed";
                }

            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteIngredientTypeAsync(int id)
        {
            var response = new ServiceResponse<bool>();
            var exist = await _unitOfWork.IngredientTypeRepository.GetByIdAsync(id);
            if (exist == null)
            {
                response.Success = false;
                response.Message = "IngredientType not found";
                return response;
            }
            try
            {
                //_unitOfWork.IngredientTypeRepository.SoftRemove(exist);
                exist.Status = UserStatus.InActive.ToString();
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Success = true;
                    response.Message = "IngredientType deleted successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "IngredientType product failed";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<IngredientTypeDTO>>> GetAllIngredientTypeAsync()
        {
            var _response = new ServiceResponse<IEnumerable<IngredientTypeDTO>>();
            try
            {
                var IngredientTypes = await _unitOfWork.IngredientTypeRepository.GetAllAsync();
                var IngredientTypeDTOs = new List<IngredientTypeDTO>();
                foreach (var pro in IngredientTypes)
                {
                    IngredientTypeDTOs.Add(_mapper.Map<IngredientTypeDTO>(pro));
                }
                if (IngredientTypeDTOs.Count != 0)
                {
                    _response.Success = true;
                    _response.Message = "IngredientType retrieved successfully";
                    _response.Data = IngredientTypeDTOs;
                }
                else
                {
                    _response.Success = true;
                    _response.Message = "IngredientType not found";
                }
            }
            catch (DbException ex)
            {
                _response.Success = false;
                _response.Message = "Database error occurred.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
        public async Task<ServiceResponse<IngredientTypeDTO>> GetIngredientTypeAsync(int id)
        {
            var _response = new ServiceResponse<IngredientTypeDTO>();
            try
            {
                var IngredientTypes = await _unitOfWork.IngredientTypeRepository.GetAsync(x => x.Id == id);
                if (IngredientTypes != null)
                {
                    _response.Success = true;
                    _response.Message = "IngredientType retrieved successfully";
                    _response.Data = _mapper.Map<IngredientTypeDTO>(IngredientTypes);
                }
                else
                {
                    _response.Success = true;
                    _response.Message = "IngredientType not found";
                }
            }
            catch (DbException ex)
            {
                _response.Success = false;
                _response.Message = "Database error occurred.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Data = null;
                _response.Message = "Error";
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<IngredientTypeDTO>> UpdateIngredientTypeAsync(int id, AddUpdateIngredientTypeDTO IngredientTypeDTO, int userId)
        {
            var response = new ServiceResponse<IngredientTypeDTO>();
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var exist = await _unitOfWork.IngredientTypeRepository.GetAsync(x => x.Id == id);
        
            if (exist == null)
            {
                response.Success = false;
                response.Message = "IngredientType not found";
                return response;
            }
            try
            {
                exist.UpdatedDate = DateTime.Now;
                exist.UpdatedBy = user.Username;
                var IngredientType = _mapper.Map(IngredientTypeDTO, exist);
                _unitOfWork.IngredientTypeRepository.Update(IngredientType);
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    response.Success = true;
                    response.Message = "IngredientType updated successfully";
                    response.Data = _mapper.Map<IngredientTypeDTO>(IngredientType);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Update IngredientType failed";
                }
            }
            catch (DbException ex)
            {
                response.Success = false;
                response.Message = "Database error occurred.";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
       
        
    }
}
