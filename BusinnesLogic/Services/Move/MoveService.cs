using AccessData.Entities;
using AccessData.Repository;
using AccessData.Repository.IMoveDetailRepository;
using App.Common.Classes.Base.Repositories;
using App.Common.Classes.Base.Services;
using AutoMapper;
using Common.Classes.BussinesLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace BusinnesLogic.Services
{
    public class MoveService : BaseService<MoveDTO, Move>, IMoveService
    {
        private IMoveRepository moveRepository;
        private IMoveDetailRepository moveDetailRepository;
        private IMapper _mapper;
        private  IValidator<MoveDTO> _moveValidate; 

        public MoveService(IBaseCRUDRepository<Move> repository, App.Common.Classes.Cache.IMemoryCacheManager
            memoryCacheManager, IMapper mapper
            , Microsoft.Extensions.Configuration.IConfiguration configuration
            , IMoveRepository moveRepository
            , IValidator<MoveDTO> moveValidate
            , IMoveDetailRepository moveDetailRepository)
            : base(repository, memoryCacheManager, mapper, configuration)
        {
            this.moveRepository = moveRepository;
            this.moveDetailRepository = moveDetailRepository;
            _mapper = mapper;
            _moveValidate = moveValidate;
        }

        public async Task<List<string>> CreateMove(MoveFileDTO file)
        {
            MoveDTO moveDTO = new MoveDTO
            {
                IdMove = 0,
                DateMove = DateTime.Now,
                IdentificationCard = file.IdentificationCard
            };

            moveDTO.MoveDetailDTO = new List<MoveDetailDTO>();

            using (var reader = new StreamReader(file.File.OpenReadStream()))
            {
                int index = 1;
                while (reader.Peek() >= 0)
                {
                    var moveDetailDTO =
                        new MoveDetailDTO
                        {
                            IdMove = moveDTO.IdMove,
                            Value = Int32.Parse(reader.ReadLine()),
                            Position = index
                        };
                    moveDTO.MoveDetailDTO.Add(moveDetailDTO);
                    index++;
                }
            }

            if (!_moveValidate.Validate(moveDTO).IsValid)
            {
                string response = "";
                foreach (var item in _moveValidate.Validate(moveDTO).Errors.ToList())
                {
                    response = response + "\n" + item;
                }
                throw new Exception(response);
            }
            List<string> listToReturn = GetMovingGrips(moveDTO.MoveDetailDTO);
            await this.moveRepository.CreateAsync(_mapperDependency.Map<Move>(moveDTO));

            return listToReturn;
        }

           
        public List<string> GetMovingGrips(List<MoveDetailDTO> moveDetailDTO)
        {


            List<string> daysDetails = new List<string>();
            int index = 0;
            //Recorremos la lista de detalles de mudanza
            for (int i = 1; i < moveDetailDTO.Count; i++)
            {
                //Obtenemos la cantidad de detalles
                int numElements = moveDetailDTO.OrderBy(o => o.Position)
                                                    .Select(x => x.Value)
                                                    .ToList().ElementAt(i);

                if ((numElements + (i +1)) > moveDetailDTO.Count)
                {
                    throw new Exception(_configuration.GetSection("ElementsOutRange").Value);
                }
                //Obtenemos el rango de la lista de detalles
                var elementsDay = moveDetailDTO.OrderBy(o => o.Position)
                                                    .Select(x => x.Value)
                                                    .ToList()
                                                    .GetRange(i + 1, numElements);
                i = i + numElements;
                index++;
                //Mostramos el resultado del caso
                daysDetails.Add("Caso #" + index + ": " + CalculateTripsDay(elementsDay));
            }
            return daysDetails;
        }

        private int CalculateTripsDay(List<int> elementsDay)
        {
            //Inicializamos la cantidad de viajes que se pueden hacer
            int travel = 0;

            //Recorremos la lista de elementos hasta que no quede ninguno
            while (elementsDay.Count > 0)
            {
                //Contador para cantidad de viajes
                var count = 1;
                //Peso en libras del viaje
                var size = 0;
                //Buscamos el elemento con peso mayor actual de la lista
                var max = elementsDay.Max();
                                            //Lo eliminamos de la lista para saber que ya fue utilizado
                elementsDay.Remove(max);
                //Verificamos si el elemento pesa menos de 50 libras, si la suma con otros elementos aún es menor a 50 libras y si aún hay elementos por utilizar
                while ((max < 50 && size < 50) && elementsDay.Count > 0)
                {
                    //Buscamos el elemento menor para relacionarlo con el elemento mayor actual que tengamos en memoria
                    var min = elementsDay.Min();//1
                                                //Removemos el elemento menor actual
                    elementsDay.Remove(min);
                    //Contamos la cantidad de elementos utilizados para en la bolsa para el viaje
                    count++;
                    //Calculamos el total que cree que lleva la supervisora con respecto al último elemento
                    size = max * count;
                }
                //Validamos que si que el tamaño o el máximo actual equivalga a más de 50 librar para generar un nuevo viaje
                if (size >= 50 || max >= 50)
                {
                    travel++;
                }
            }
            return travel;
        }
    }

    public class ValidatorMoveService : AbstractValidator<MoveDTO> {

        private Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public ValidatorMoveService(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;

            RuleFor(x => x.MoveDetailDTO)
                .Custom((list, context) => {

                    if (list.Select(s => s.Value).FirstOrDefault() > 500)
                    {
                        context.AddFailure(_configuration
                               .GetSection("DayOutOfRange").Value);
                    }

                    bool has = list.Where(o => o.Position > 1).Any(cus => cus.Value > 100);
                    if (has)
                    {
                        context.AddFailure(_configuration
                                .GetSection("NumberOfElementsOrWeightOutOfRange").Value);
                    }
                });


            RuleFor(z => z.IdentificationCard)
                .NotEmpty()
                .WithMessage(_configuration
                .GetSection("DocumentCard").Value);

            RuleFor(z => z.IdentificationCard)
                .NotNull()
                .WithMessage(_configuration
                .GetSection("DocumentCard").Value); ;
        }
    }
 }


